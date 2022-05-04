using System;
using System.Threading.Tasks;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Models.Reports;

namespace RookieOnlineAssetManagement.Services
{
	public interface IReportService
	{
		Task<PagedResult<ReportModel>> GetAllPaging(string accountId, GetReportPagingRequest request);
	}
}

