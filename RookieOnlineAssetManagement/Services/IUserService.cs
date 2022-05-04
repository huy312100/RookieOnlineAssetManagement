using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Models.Users;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Services
{
    public interface IUserService
    {
        public Task<string> Create(string accountId, CreateUserRequest request);

        public Task<UserModel> GetUserById(string id);

        public Task<bool> ChangePasswordFirstTime(ChangePasswordFirstTimeModel req);

        public Task<string> Update(string id, UpdateUserRequest request);

        public Task<PagedResult<UserModel>> GetAllPaging(string accountId, GetUserPagingRequest request);

        Task<int> ChangePassword(ChangePasswordRequest req);

        Task<UserModel> GetUserLogin(string username);
        // Task<User> GetUserLogin(string username);

        public Task<User> DisableUser(string username);
        public Task<bool> GetAllAssignmentsByUsername(string username);
    }
}