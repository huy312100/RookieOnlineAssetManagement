using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Models.ReturnAssets
{
    public class GetReturnAssetPagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        public List<ReturnAssetState>? States { get; set; }
        public DateTime? ReturnedDateFrom { get; set; }
        public DateTime? ReturnedDateTo { get; set; }
    }
}
