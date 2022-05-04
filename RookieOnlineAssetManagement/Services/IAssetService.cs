using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Assets;
using RookieOnlineAssetManagement.Models.Common;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Services
{
    public interface IAssetService
    {
        public Task<PagedResult<AssetModel>> GetAllPaging(string accountId, GetAssetPagingRequest request);

        public Task<PagedResult<AssetHistoryModel>> GetAssetHistoryPaging(int assetId, PagingRequestBase request);

        public Task<AssetModel> GetById(int id);

        public Task<int> Create(string accountId, CreateAssetRequest request);

        public Task<string> Update(int assetId, UpdateAssetRequest request);

        public Task<bool> CheckAssetHistory(int assetId);
        public Task<Asset> DeleteAsset(int assetId);
    }
}