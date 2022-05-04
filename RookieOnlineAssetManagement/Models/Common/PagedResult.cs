using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Models.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { get; set; }
    }
}