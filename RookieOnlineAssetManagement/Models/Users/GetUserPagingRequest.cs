using RookieOnlineAssetManagement.Models.Common;

namespace RookieOnlineAssetManagement.Models.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        public string? Type { get; set; }
    }
}