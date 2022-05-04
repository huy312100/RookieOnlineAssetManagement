using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Users;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Utilities.Constants;
using RookieOnlineAssetManagement.Models.Common;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> Create(string accountId, CreateUserRequest request)
        {
            var adminAccount = await _userManager.FindByIdAsync(accountId);
            if (adminAccount == null)
            {
                return SystemConstants.APIReturnPermissionDenied;
            }
            var locationAdmin = adminAccount.LocationId;

            Regex sWhitespace = new Regex(@"\s+");
            var firstName = sWhitespace.Replace(request.FirstName, "");
            firstName = firstName.ToLower();

            var lastName = request.LastName.Trim();
            lastName = lastName.ToLower();
            var lastNameWords = lastName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var word in lastNameWords)
            {
                sb.Append(word.Substring(0, 1));
            }

            var username = firstName + sb.ToString();
            var userDuplicates = await _context.Users.Where(u => u.UserName.StartsWith(username)).ToListAsync();
            var countUserDuplicate = userDuplicates.Count;
            if (countUserDuplicate != 0)
            {
                username += countUserDuplicate;
            }

            var passwordDefault = $"{username}@{request.BirthDay.Day.ToString().PadLeft(2, '0')}{request.BirthDay.Month.ToString().PadLeft(2, '0')}{request.BirthDay.Year}";

            var userInsert = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDay = request.BirthDay,
                JoinedDate = request.JoinedDate,
                Gender = request.Gender,
                UserName = username,
                LocationId = locationAdmin,
                Status = Enums.Status.Active,
                IsNew = true,
                ModifiedDate = DateTime.Now
            };

            var isAddToUserTable = await _userManager.CreateAsync(userInsert, passwordDefault);
            if (isAddToUserTable.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(userInsert.UserName);
                await _userManager.AddToRoleAsync(currentUser, request.Type);
                return currentUser.Id;
            }

            return "Create User Failed";
        }

        public async Task<PagedResult<UserModel>> GetAllPaging(string accountId, GetUserPagingRequest request)
        {
            // get account loged in
            var accountAdmin = await _userManager.FindByIdAsync(accountId);
            if (accountAdmin == null)
            {
                var pageResultFailed = new PagedResult<UserModel>
                {
                    TotalRecord = 0,
                    Items = new List<UserModel>(),
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                };
                return pageResultFailed;
            }
            // filter by location of admin
            var query = from u in _context.Users
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where u.LocationId == accountAdmin.LocationId && u.Id != accountId
                        select new { u, ur, r };

            // filter by type
            if (!string.IsNullOrEmpty(request.Type))
            {
                query = query.Where(u => (u.r.NormalizedName == request.Type || u.r.Name == request.Type));
            }
            // search by keyword fullname or staff code
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(user => (user.u.FirstName + " " + user.u.LastName).Contains(request.Keyword) || user.u.UserCode.Contains(request.Keyword));
            }

            var totalRow = await query.CountAsync();
            switch (request.OrderName)
            {
                case "modified":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.u.ModifiedDate);
                            break;
                        }
                        else
                        {
                            query = query.OrderBy(x => x.u.ModifiedDate);
                            break;
                        }
                    }
                case "type":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.r.Name);
                            break;
                        }
                        else
                        {
                            query = query.OrderBy(x => x.r.Name);
                            break;
                        }
                    }
                case "fullName":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.u.FirstName).ThenByDescending(x => x.u.LastName);
                            break;
                        }
                        else
                        {
                            query = query.OrderBy(x => x.u.FirstName).ThenBy(x => x.u.LastName);
                            break;
                        }
                    }
                case "joinedDate":
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.u.JoinedDate);
                            break;
                        }
                        else
                        {
                            query = query.OrderBy(x => x.u.JoinedDate);
                            break;
                        }
                    }
                default:
                    {
                        if (request.OrderMode == Enums.OrderMode.DES)
                        {
                            query = query.OrderByDescending(x => x.u.UserCode);
                            break;
                        }
                        else
                        {
                            query = query.OrderBy(x => x.u.UserCode);
                            break;
                        }
                    }
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(user => new UserModel()
            {
                Id = user.u.Id,
                UserName = user.u.UserName,
                FullName = user.u.FirstName + " " + user.u.LastName,
                StaffCode = user.u.UserCode,
                BirthDay = user.u.BirthDay,
                Gender = user.u.Gender,
                JoinedDate = user.u.JoinedDate,
                Location = accountAdmin.LocationId,
                Type = user.r.Name,
                IsNew = user.u.IsNew,
                ModifiedDate = user.u.ModifiedDate
            }).ToListAsync();
            var pageResult = new PagedResult<UserModel>
            {
                TotalRecord = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return pageResult;
        }

        public async Task<UserModel> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userVm = new UserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    BirthDay = user.BirthDay,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Gender = user.Gender,
                    JoinedDate = user.JoinedDate,
                    StaffCode = user.UserCode,
                    Location = user.LocationId,
                    IsNew = user.IsNew,
                    ModifiedDate = user.ModifiedDate
                };
                var roleOfUser = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                userVm.Type = roleOfUser;
                return userVm;
            }
            return null;
        }

        public async Task<string> Update(string id, UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return SystemConstants.APIFindUserByIdErrorInValidId;
            }
            user.BirthDay = request.BirthDay;
            user.JoinedDate = request.JoinedDate;
            user.Gender = request.Gender;
            user.ModifiedDate = DateTime.Now;
            var updateUserResult = await _userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                return SystemConstants.APIUpdateUserReturnError;
            }
            var oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
            if (!removeRoleResult.Succeeded)
            {
                return SystemConstants.APIUpdateUserReturnError;
            }
            var addNewRoleResult = await _userManager.AddToRoleAsync(user, request.Type);
            if (addNewRoleResult.Succeeded)
            {
                return SystemConstants.APIUpdateUserReturnSuccess;
            }
            return SystemConstants.APIUpdateUserReturnError;
        }

        public async Task<int> ChangePassword(ChangePasswordRequest req)
        {
            var user = await _context.Users.FindAsync(req.UserId);
            if (user != null && req.PasswordNew != "" && req.PasswordOld != "")
            {
                PasswordHasher<User> hash = new PasswordHasher<User>();
                var verifyPasswordResult = hash.VerifyHashedPassword(user, user.PasswordHash, req.PasswordOld);

                if (verifyPasswordResult.ToString() == "Success")
                {
                    var hashPassword = hash.HashPassword(user, req.PasswordNew);
                    user.PasswordHash = hashPassword;
                    _context.SaveChanges();

                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }

        public async Task<UserModel> GetUserLogin(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            var userVm = new UserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                IsNew = user.IsNew,
            };
            var roleOfUser = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            userVm.Type = roleOfUser;

            return userVm;
        }

        public async Task<bool> ChangePasswordFirstTime(ChangePasswordFirstTimeModel req)
        {
            var user = await _context.Users.FindAsync(req.UserId);
            if (user != null && user.IsNew && req.PasswordNew != "")
            {
                PasswordHasher<User> hash = new PasswordHasher<User>();
                var hashPassword = hash.HashPassword(user, req.PasswordNew);
                user.IsNew = false;
                user.PasswordHash = hashPassword;
                return await _context.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        public async Task<User> DisableUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return user;
            }
            return null;
        }

        public async Task<bool> GetAllAssignmentsByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _context.Assignments.Where(s => s.UserId == user.Id).ToListAsync();
                if (result.Count() == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}