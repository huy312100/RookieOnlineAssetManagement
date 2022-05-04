using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Models.Reports;

namespace RookieOnlineAssetManagement.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReportService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PagedResult<ReportModel>> GetAllPaging(string accountId, GetReportPagingRequest request)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            if (adminAccount == null)
            {
                return new PagedResult<ReportModel>()
                {
                    Items = null,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecord = 0
                };
            }

            var categories = _context.Categories.AsQueryable().Include(x => x.Assets);

            var query = categories.Select(x => new ReportModel()
            {
                Category = x.CategoryName,
                Total = x.Assets.Where(a => a.LocationId == adminAccount.LocationId).ToList().Count,
                Assigned = x.Assets.Where(a => a.AssetState == AssetState.Assigned
                            && a.LocationId == adminAccount.LocationId).ToList().Count,
                Available = x.Assets.Where(a => a.AssetState == AssetState.Available
                            && a.LocationId == adminAccount.LocationId).ToList().Count,
                NotAvailable = x.Assets.Where(a => a.AssetState == AssetState.NotAvailable
                            && a.LocationId == adminAccount.LocationId).ToList().Count,
                WaitingForRecycling = x.Assets.Where(a => a.AssetState == AssetState.WaitingForRecycling
                            && a.LocationId == adminAccount.LocationId).ToList().Count,
                Recycled = x.Assets.Where(a => a.AssetState == AssetState.Recycled
                            && a.LocationId == adminAccount.LocationId).ToList().Count
            });

            // order
            switch (request.OrderName)
            {
                case "category":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.Category);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Category);
                        }
                        break;
                    }
                case "total":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.Total);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Total);
                        }
                        break;
                    }
                case "assigned":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.Assigned);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Assigned);
                        }
                        break;
                    }
                case "available":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.Available);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Available);
                        }
                        break;
                    }
                case "notAvailable":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.NotAvailable);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.NotAvailable);
                        }
                        break;
                    }
                case "waitingForRecycling":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.WaitingForRecycling);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.WaitingForRecycling);
                        }
                        break;
                    }
                case "recycled":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.Recycled);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Recycled);
                        }
                        break;
                    }
                default:
                    break;
            }

            // paging
            var totalRow = await query.CountAsync();
            List<ReportModel> data;

            if (request.GetAll)
            {
               data  = await query.ToListAsync();
            }
            else
            {
               data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize).ToListAsync();
            }

            var pageResult = new PagedResult<ReportModel>()
            {
                Items = data,
                PageIndex = request.GetAll ? 0 : request.PageIndex ,
                PageSize = request.GetAll ? 0 : request.PageSize,
                TotalRecord = totalRow
            };
            return pageResult;

        }
    }
}

