using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Assets;
using RookieOnlineAssetManagement.Models.Common;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using RookieOnlineAssetManagement.Utilities.Constants;

namespace RookieOnlineAssetManagement.Services
{
    public class AssetService : IAssetService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AssetService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Create(string accountId, CreateAssetRequest request)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            if (adminAccount == null)
            {
                return -1;
            }
            var locationAdmin = adminAccount.LocationId;
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return -2;
            }
            var prefix = category.Prefix;

            var countExisted = await _context.Assets.Where(x => x.AssetCode.Contains(prefix)).CountAsync();
            var assetCode = prefix + (countExisted + 1).ToString().PadLeft(6, '0');
            var assetDuplicate = _context.Assets.Where(x => x.AssetCode == assetCode).FirstOrDefault();
            while (assetDuplicate != null)
            {
                countExisted++;
                assetCode = prefix + (countExisted + 1).ToString().PadLeft(6, '0');
                assetDuplicate = _context.Assets.Where(x => x.AssetCode == assetCode).FirstOrDefault();
            }
            var asset = new Asset()
            {
                AssetCode = assetCode,
                AssetName = request.AssetName,
                AssetState = request.AssetState,
                InstalledDate = request.InstalledDate,
                CategoryId = request.CategoryId,
                ModifiedDate = DateTime.Now,
                Specification = request.Specification,
                Status = Enums.Status.Active,
                LocationId = locationAdmin
            };
            _context.Assets.Add(asset);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var assetCreated = _context.Assets.Where(x => x.AssetCode == assetCode).FirstOrDefault();
                return assetCreated.Id;
            }
            return 0;
        }

        public async Task<PagedResult<AssetModel>> GetAllPaging(string accountId, GetAssetPagingRequest request)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            if (adminAccount == null)
            {
                return new PagedResult<AssetModel>()
                {
                    Items = null,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecord = 0
                };
            }
            var query = from a in _context.Assets
                        join c in _context.Categories on a.CategoryId equals c.Id
                        where a.LocationId == adminAccount.LocationId
                        select new { a, c };
            // filter state
            if (request.States != null && request.States.Count > 0)
            {
                query = query.Where(asset => request.States.Contains(asset.a.AssetState));
            }
            // filter category
            if (request.Categories != null && request.Categories.Count > 0)
            {
                query = query.Where(asset => request.Categories.Contains(asset.c.Id));
            }
            // search by name or asset code
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(asset => asset.a.AssetName.Contains(request.Keyword) || asset.a.AssetCode.Contains(request.Keyword));
            }
            // order
            switch (request.OrderName)
            {
                case "modified":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.a.ModifiedDate);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.a.ModifiedDate);
                        }
                        break;
                    }
                case "assetCode":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.a.AssetCode);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.a.AssetCode);
                        }
                        break;
                    }
                case "assetName":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.a.AssetName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.a.AssetName);
                        }
                        break;
                    }
                case "category":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.c.CategoryName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.c.CategoryName);
                        }
                        break;
                    }
                case "state":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x =>
                            x.a.AssetState == Enums.AssetState.WaitingForRecycling ? Enums.AssetState.WaitingForRecycling.ToString() :
                            x.a.AssetState == Enums.AssetState.Recycled ? Enums.AssetState.Recycled.ToString() :
                            x.a.AssetState == Enums.AssetState.NotAvailable ? Enums.AssetState.NotAvailable.ToString() :
                            x.a.AssetState == Enums.AssetState.Available ? Enums.AssetState.Available.ToString() :
                            Enums.AssetState.Assigned.ToString()
                            );
                        }
                        else
                        {
                            query = query.OrderBy(x =>
                            x.a.AssetState == Enums.AssetState.Assigned ? Enums.AssetState.Assigned.ToString() :
                            x.a.AssetState == Enums.AssetState.Available ? Enums.AssetState.Available.ToString() :
                            x.a.AssetState == Enums.AssetState.NotAvailable ? Enums.AssetState.NotAvailable.ToString() :
                            x.a.AssetState == Enums.AssetState.Recycled ? Enums.AssetState.Recycled.ToString() :
                            Enums.AssetState.WaitingForRecycling.ToString()
                            );
                        }
                        break;
                    }
                default:
                    break;
            }
            // paging
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new AssetModel()
                {
                    Id = x.a.Id,
                    AssetCode = x.a.AssetCode,
                    AssetName = x.a.AssetName,
                    AssetState = x.a.AssetState,
                    CategoryName = x.c.CategoryName,
                    InstalledDate = x.a.InstalledDate,
                    LocationId = x.a.LocationId,
                    Specification = x.a.Specification
                }).ToListAsync();

            var pageResult = new PagedResult<AssetModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pageResult;
        }

        public async Task<PagedResult<AssetHistoryModel>> GetAssetHistoryPaging(int assetId, PagingRequestBase request)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null)
            {
                return new PagedResult<AssetHistoryModel>()
                {
                    Items = null,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecord = 0
                };
            }
            var query = from a in _context.Assets
                        join am in _context.Assignments on a.Id equals am.AssetId
                        join ub in _context.Users on am.AssignerId equals ub.Id
                        join ut in _context.Users on am.UserId equals ut.Id
                        join ra in _context.ReturnAssets on am.Id equals ra.AssignmentId into aam
                        from mr in aam.DefaultIfEmpty()
                        where a.Id == assetId
                        select new { am, mr, ub, ut };
            var totalRow = await query.CountAsync();
            var data = await query.OrderBy(x => x.am.AssignedDate).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new AssetHistoryModel()
                {
                    AssignedDate = x.am.AssignedDate,
                    AssignedBy = x.ub.UserName,
                    AssignedTo = x.ut.UserName,
                    ReturnedDate = x.mr.ReturnedDate
                }).ToListAsync();
            var pageResult = new PagedResult<AssetHistoryModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pageResult;
        }

        public async Task<AssetModel> GetById(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                var assetVm = new AssetModel()
                {
                    Id = asset.Id,
                    AssetCode = asset.AssetCode,
                    AssetName = asset.AssetName,
                    AssetState = asset.AssetState,
                    InstalledDate = asset.InstalledDate,
                    LocationId = asset.LocationId,
                    Specification = asset.Specification
                };
                var categoryOfAsset = await _context.Categories.FindAsync(asset.CategoryId);
                assetVm.CategoryName = categoryOfAsset.CategoryName;
                return assetVm;
            }
            return null;
        }

        public async Task<bool> CheckAssetHistory(int assetId)
        {
            var assignment = await _context.Assignments.Where(s => s.AssetId == assetId).ToListAsync();
            if (assignment.Count > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Asset> DeleteAsset(int assetId)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(e => e.Id == assetId);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
                return asset;
            }
            return null;
        }

        public async Task<string> Update(int assetId, UpdateAssetRequest request)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null)
            {
                return SystemConstants.APIFindAssetByIdErrorInValidId;
            }

            asset.AssetName = request.AssetName;
            asset.Specification = request.Specification;
            asset.InstalledDate = request.InstalledDate;
            asset.AssetState = request.AssetState;

            _context.Assets.Update(asset);

            var updateAssetResult = await _context.SaveChangesAsync();
            if (updateAssetResult > 0)
            {
                return SystemConstants.APIUpdateAssetReturnSuccess;
            }
            return SystemConstants.APIUpdateAssetReturnError;
        }
    }
}