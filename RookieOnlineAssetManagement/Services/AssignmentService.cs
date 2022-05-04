using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Assignments;
using System.Threading.Tasks;
using System.Linq;
using System;
using RookieOnlineAssetManagement.Models.Common;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Utilities.Constants;

namespace RookieOnlineAssetManagement.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AssignmentService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Create(string accountId, CreateAssignmentRequest request)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            if (adminAccount == null)
            {
                return -1;
            }
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return -2;
            }
            var asset = await _context.Assets.FindAsync(request.AssetId);
            if (asset == null)
            {
                return -3;
            }
            var assignment = new Assignment()
            {
                AssetId = request.AssetId,
                AssignedDate = request.AssignedDate,
                AssignerId = adminAccount.Id,
                ModifiedDate = DateTime.Now,
                Note = request.Note,
                UserId = request.UserId,
                AssignmentState = Enums.AssignmentState.WaitingForAcceptance
            };
            _context.Assignments.Add(assignment);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var assignmentCreated = _context.Assignments.Where(x => x == assignment).FirstOrDefault();
                // update asset state after assign success
                var currentAsset = await _context.Assets.FindAsync(request.AssetId);
                currentAsset.AssetState = Enums.AssetState.Assigned;
                await _context.SaveChangesAsync();
                return assignmentCreated.Id;
            }
            return 0;
        }

        public async Task<PagedResult<AssignmentModel>> GetAllPaging(string accountId, GetAssignmentPagingRequest request)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            if (adminAccount == null)
            {
                return new PagedResult<AssignmentModel>()
                {
                    Items = null,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecord = 0
                };
            }
            var query = from am in _context.Assignments
                        join ub in _context.Users on am.AssignerId equals ub.Id
                        join ut in _context.Users on am.UserId equals ut.Id
                        join at in _context.Assets on am.AssetId equals at.Id
                        join ra in _context.ReturnAssets on am.Id equals ra.AssignmentId into aam
                        from mr in aam.DefaultIfEmpty()
                        where at.LocationId == adminAccount.LocationId && (mr == null || mr.ReturnAssetState != Enums.ReturnAssetState.Completed)
                        select new { am, at, ut, ub };
            if (request.States != null && request.States.Count > 0)
            {
                query = query.Where(x => request.States.Contains(x.am.AssignmentState));
            }
            if (request.AssignedDateFrom != null)
            {
                query = query.Where(x => x.am.AssignedDate >= request.AssignedDateFrom);
            }
            if (request.AssignedDateTo != null)
            {
                query = query.Where(x => x.am.AssignedDate < request.AssignedDateTo);
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
                            query = query.OrderByDescending(x => x.am.Id);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.am.Id);
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
                case "assignedTo":
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
                case "assignedBy":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.ub.UserName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.ub.UserName);
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
                case "state":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x =>
                            x.am.AssignmentState == Enums.AssignmentState.WaitingForAcceptance ? Enums.AssignmentState.WaitingForAcceptance.ToString() :
                            Enums.AssignmentState.Accepted.ToString()
                            );
                        }
                        else
                        {
                            query = query.OrderBy(x =>
                            x.am.AssignmentState == Enums.AssignmentState.Accepted ? Enums.AssignmentState.Accepted.ToString() :
                            Enums.AssignmentState.WaitingForAcceptance.ToString()
                            );
                        }
                        break;
                    }
                default:
                    break;
            }

            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new AssignmentModel()
                {
                    Id = x.am.Id,
                    UserIdAssigned = x.ut.Id,
                    AssetId = x.at.Id,
                    FullnameAssigned = x.ut.FirstName + " " + x.ut.LastName,
                    AssetCode = x.at.AssetCode,
                    AssetName = x.at.AssetName,
                    AssignedBy = x.ub.UserName,
                    AssignedTo = x.ut.UserName,
                    AssignedDate = x.am.AssignedDate,
                    AssignmentState = x.am.AssignmentState,
                    ModifiedDate = x.am.ModifiedDate,
                    Note = x.am.Note,
                    Specification = x.at.Specification,
                    ReturnAsset = x.am.ReturnAssets.FirstOrDefault()
                }).ToListAsync();
            var pageResult = new PagedResult<AssignmentModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pageResult;
        }

        public AssignmentModel GetById(int id)
        {
            var query = from a in _context.Assignments
                        join asset in _context.Assets on a.AssetId equals asset.Id
                        join ut in _context.Users on a.UserId equals ut.Id
                        join ub in _context.Users on a.AssignerId equals ub.Id
                        where a.Id == id
                        select new { a, asset, ut, ub };
            var assignment = query.FirstOrDefault();
            if (assignment != null)
            {
                var assignmentVm = new AssignmentModel()
                {
                    Id = assignment.a.Id,
                    UserIdAssigned = assignment.ut.Id,
                    AssetId = assignment.asset.Id,
                    FullnameAssigned = assignment.ut.FirstName + " " + assignment.ut.LastName,
                    AssignedDate = assignment.a.AssignedDate,
                    AssignmentState = assignment.a.AssignmentState,
                    ModifiedDate = assignment.a.ModifiedDate,
                    Note = assignment.a.Note,
                    AssetCode = assignment.asset.AssetCode,
                    AssetName = assignment.asset.AssetName,
                    AssignedBy = assignment.ub.UserName,
                    AssignedTo = assignment.ut.UserName,
                    Specification = assignment.asset.Specification,
                };
                return assignmentVm;
            }
            return null;
        }

        public async Task<Assignment> DeleteAssignment(string accountId, int assignmentId)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            var checkRole = await _userManager.IsInRoleAsync(adminAccount, "Admin");

            var assignment = await _context.Assignments.FirstOrDefaultAsync(e => e.Id == assignmentId && (e.UserId == accountId || e.AssignerId == accountId || checkRole == true));
            if (assignment != null)
            {
                var asset = await _context.Assets.FirstOrDefaultAsync(x => x.Id == assignment.AssetId);
                var assignments = await _context.Assignments.Where(x => x.AssetId == asset.Id).ToListAsync();
                if (assignments.Count == 1)
                {
                    asset.AssetState = Enums.AssetState.Available;
                    _context.Assets.Update(asset);
                }

                _context.Assignments.Remove(assignment);

                var returnAsset = await _context.ReturnAssets.FirstOrDefaultAsync(x => x.AssignmentId == assignment.Id);
                if (returnAsset != null)
                {
                    _context.ReturnAssets.Remove(returnAsset);
                }

                await _context.SaveChangesAsync();
                return assignment;
            }
            return null;
        }

        public async Task<PagedResult<AssignmentModel>> GetHomePaging(string accountId, GetHomePagingRequest request)
        {
            var userAccount = await _userManager.FindByIdAsync(accountId);
            if (userAccount == null)
            {
                return new PagedResult<AssignmentModel>()
                {
                    Items = null,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecord = 0
                };
            }
            var query = from am in _context.Assignments
                        join ub in _context.Users on am.AssignerId equals ub.Id
                        join ut in _context.Users on am.UserId equals ut.Id
                        join at in _context.Assets on am.AssetId equals at.Id
                        join c in _context.Categories on at.CategoryId equals c.Id
                        join ra in _context.ReturnAssets on am.Id equals ra.AssignmentId into aam
                        from mr in aam.DefaultIfEmpty()
                        where (am.UserId == accountId && am.AssignedDate <= DateTime.Now && (mr == null || mr.ReturnAssetState != Enums.ReturnAssetState.Completed))
                        select new { am, at, ut, ub, c };

            switch (request.OrderName)
            {
                case "id":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.am.Id);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.am.Id);
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
                case "state":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x =>
                            x.am.AssignmentState == Enums.AssignmentState.WaitingForAcceptance ? Enums.AssignmentState.WaitingForAcceptance.ToString() :
                            Enums.AssignmentState.Accepted.ToString()
                            );
                        }
                        else
                        {
                            query = query.OrderBy(x =>
                            x.am.AssignmentState == Enums.AssignmentState.Accepted ? Enums.AssignmentState.Accepted.ToString() :
                            Enums.AssignmentState.WaitingForAcceptance.ToString()
                            );
                        }
                        break;
                    }
                default:
                    break;
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new AssignmentModel()
                {
                    Id = x.am.Id,
                    AssetCode = x.at.AssetCode,
                    AssetName = x.at.AssetName,
                    CategoryName = x.c.CategoryName,
                    AssignedDate = x.am.AssignedDate,
                    AssignmentState = x.am.AssignmentState,
                    Note = x.am.Note,
                    Specification = x.at.Specification,
                    AssignedBy = x.ub.UserName,
                    AssignedTo = x.ut.UserName,
                    ReturnAsset = x.am.ReturnAssets.FirstOrDefault()
                })
                .ToListAsync();
            var pageResult = new PagedResult<AssignmentModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pageResult;
        }

        public async Task<Assignment> ChangeAssignmentState(int assignmentId)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(x => x.Id == assignmentId);
            if (assignment != null)
            {
                assignment.AssignmentState = Enums.AssignmentState.Accepted;
                _context.Assignments.Update(assignment);

                var returnAsset = await _context.ReturnAssets.FirstOrDefaultAsync(x => x.AssignmentId == assignment.Id);
                if (returnAsset != null)
                {
                    _context.ReturnAssets.Remove(returnAsset);
                }

                await _context.SaveChangesAsync();
                return assignment;
            }
            return null;
        }

        public async Task<string> Update(int assignmentId, UpdateAssignmentRequest request)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            var user = await _context.Users.FindAsync(request.UserId);
            var newAsset = await _context.Assets.FindAsync(request.NewAssetId);
            var oldAsset = await _context.Assets.FindAsync(request.OldAssetId);

            if (assignment == null)
            {
                return SystemConstants.APIFindAssignmentByIdErrorInValidId;
            }
            if (user == null)
            {
                return SystemConstants.APIFindUserByIdErrorInValidId;
            }
            if (newAsset == null)
            {
                return SystemConstants.APIFindAssetByIdErrorInValidId;
            }

            assignment.UserId = request.UserId;
            assignment.AssetId = request.NewAssetId;
            assignment.AssignedDate = request.AssignedDate;
            assignment.Note = request.Note;

            oldAsset.AssetState = Enums.AssetState.Available;
            newAsset.AssetState = Enums.AssetState.Assigned;

            _context.Assets.Update(oldAsset);
            _context.Assets.Update(newAsset);

            _context.Assignments.Update(assignment);

            var updateAssignmentResult = await _context.SaveChangesAsync();
            if (updateAssignmentResult > 0)
            {
                return SystemConstants.APIUpdateAssignmentReturnSuccess;
            }
            return SystemConstants.APIUpdateAssignmentReturnError;
        }
    }
}