using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models.Common;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class GetHomePagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        public List<AssignmentState>? States { get; set; }

    }
}
