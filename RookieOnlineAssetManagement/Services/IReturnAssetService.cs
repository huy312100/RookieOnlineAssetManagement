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
    public interface IReturnAssetService
    {
        public Task<PagedResult<ReturnAssetModel>> GetAllPaging(string accountId, GetReturnAssetPagingRequest request);
        public Task<ReturnAsset> CreateRequestReturnAssetUser(int assingmentId);

        public Task<ReturnAsset> AcceptRequestReturnAsset(string accountId, int returnAssetId);

        public Task<ReturnAsset> CancelRequestReturnAsset(int returnAssetId);
    }
}
