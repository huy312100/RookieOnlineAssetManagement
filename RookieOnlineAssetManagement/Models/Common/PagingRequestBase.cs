using RookieOnlineAssetManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace RookieOnlineAssetManagement.Models.Common
{
    public class PagingRequestBase
    {
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "PageIndex must greater than 1")]
        public int PageIndex { get; set; }

        [Range(minimum: 1, maximum: 30, ErrorMessage = "PageSize not allowed")]
        public int PageSize { get; set; }

        public string OrderName { get; set; }
        public OrderMode OrderMode { get; set; }
    }
}