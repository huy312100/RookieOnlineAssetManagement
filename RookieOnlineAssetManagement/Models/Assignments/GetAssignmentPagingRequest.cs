using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models.Common;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class GetAssignmentPagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        public List<AssignmentState>? States { get; set; }
        public DateTime? AssignedDateFrom { get; set; }
        public DateTime? AssignedDateTo { get; set; }
    }
}