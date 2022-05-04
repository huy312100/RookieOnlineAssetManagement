using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Assignments;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Models.ReturnAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Services
{
    public class ReturnAssetService : IReturnAssetService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReturnAssetService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PagedResult<ReturnAssetModel>> GetAllPaging(string accountId, GetReturnAssetPagingRequest request)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            if (adminAccount == null)
            {
                return new PagedResult<ReturnAssetModel>()
                {
                    Items = null,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecord = 0
                };
            }
            var query = from am in _context.Assignments
                        join ut in _context.Users on am.UserId equals ut.Id
                        join at in _context.Assets on am.AssetId equals at.Id
                        join ra in _context.ReturnAssets on am.Id equals ra.AssignmentId
                        where at.LocationId == adminAccount.LocationId
                        select new { am, at, ut, ra };

            if (request.States != null && request.States.Count > 0)
            {
                query = query.Where(x => request.States.Contains(x.ra.ReturnAssetState));
            }
            if (request.ReturnedDateFrom != null)
            {
                query = query.Where(x => x.ra.ReturnedDate >= request.ReturnedDateFrom);
            }
            if (request.ReturnedDateTo != null)
            {
                query = query.Where(x => x.ra.ReturnedDate < request.ReturnedDateTo);
            }
            if (request.Keyword != null)
            {
                query = query.Where(x => x.at.AssetCode.Contains(request.Keyword) || x.at.AssetName.Contains(request.Keyword) || x.ut.UserName.Contains(request.Keyword));
            }
                switch (request.OrderName)
            {
                case "id":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.ra.Id);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.ra.Id);
                        }
                        break;
                    }
                case "assetCode":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.at.AssetCode);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.at.AssetCode);
                        }
                        break;
                    }
                case "assetName":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.at.AssetName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.at.AssetName);
                        }
                        break;
                    }
                case "requestedBy":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.ut.UserName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.ut.UserName);
                        }
                        break;
                    }
                case "assignedDate":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.am.AssignedDate);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.am.AssignedDate);
                        }
                        break;
                    }
                case "acceptedBy":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.ra.User.UserName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.ra.User.UserName);
                        }
                        break;
                    }
                case "returnedDate":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.ra.ReturnedDate);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.ra.ReturnedDate);
                        }
                        break;
                    }
                case "state":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderBy(x => x.ra.ReturnAssetState);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ra.ReturnAssetState);
                        }
                        break;
                    }
                default:
                    break;
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ReturnAssetModel()
                {
                    Id = x.ra.Id,
                    AssetCode = x.at.AssetCode,
                    AssetName = x.at.AssetName,
                    RequestedBy = x.ut.UserName,
                    AssignedDate = x.am.AssignedDate,
                    AcceptedBy = x.ra.User.UserName,
                    ReturnedDate = x.ra.ReturnedDate,
                    ReturnAssetState = x.ra.ReturnAssetState
                }).ToListAsync();
            var pageResult = new PagedResult<ReturnAssetModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pageResult;
        }

        public async Task<ReturnAsset> CreateRequestReturnAssetUser(int assignmentId)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(x => x.Id == assignmentId);
            if (assignment != null)
            {
                var requestReturnAsset = new ReturnAsset()
                {
                    UserId = null,
                    AssignmentId = assignment.Id,
                    ModifiedDate = DateTime.Now,
                    ReturnAssetState = Enums.ReturnAssetState.WaitingForReturning
                };
                _context.ReturnAssets.Add(requestReturnAsset);
                await _context.SaveChangesAsync();
                return requestReturnAsset;
            }
            return null;
        }

        public async Task<ReturnAsset> AcceptRequestReturnAsset(string accountId, int returnAssetId)
        {
            var returnAsset = await _context.ReturnAssets.FirstOrDefaultAsync(x => x.Id == returnAssetId);
            if (returnAsset != null)
            {
                var assignment = await _context.Assignments.FirstOrDefaultAsync(x => x.Id == returnAsset.AssignmentId);
                var asset = await _context.Assets.FirstOrDefaultAsync(x => x.Id == assignment.AssetId);
                returnAsset.ReturnAssetState = Enums.ReturnAssetState.Completed;
                returnAsset.ReturnedDate = DateTime.Now;
                returnAsset.UserId = accountId;
                _context.ReturnAssets.Update(returnAsset);
                asset.AssetState = Enums.AssetState.Available;
                _context.Assets.Update(asset);

                await _context.SaveChangesAsync();
                return returnAsset;
            }
            return null;
        }

        public async Task<ReturnAsset> CancelRequestReturnAsset(int returnAssetId)
        {

            var returnAsset = await _context.ReturnAssets.FirstOrDefaultAsync(e => e.Id == returnAssetId);
            if (returnAsset != null)
            {
                _context.ReturnAssets.Remove(returnAsset);
                await _context.SaveChangesAsync();
                return returnAsset;
            }
            return null;
        }


    }
}
