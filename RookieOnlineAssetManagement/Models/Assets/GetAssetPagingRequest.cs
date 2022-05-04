using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models.Common;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Models.Assets
{
    public class GetAssetPagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        public List<AssetState>? States { get; set; }
        public List<int>? Categories { get; set; }
    }
}