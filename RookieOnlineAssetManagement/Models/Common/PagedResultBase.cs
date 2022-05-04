using System;

namespace RookieOnlineAssetManagement.Models.Common
{
    public class PagedResultBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecord { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecord / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
}