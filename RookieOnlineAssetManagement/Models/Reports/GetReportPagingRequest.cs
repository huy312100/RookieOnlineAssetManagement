using System;
using RookieOnlineAssetManagement.Models.Common;

namespace RookieOnlineAssetManagement.Models.Reports
{
	public class GetReportPagingRequest : PagingRequestBase
	{
		public bool GetAll { get; set; }
	}
}

