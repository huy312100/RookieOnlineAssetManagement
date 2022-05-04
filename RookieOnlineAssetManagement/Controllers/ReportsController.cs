using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Models.Reports;
using RookieOnlineAssetManagement.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("paging")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetReportPagingRequest request)
        {
            var accountId = User.Claims.Where(c => c.Type ==
                System.Security.Claims.ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault();

            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }
            var result = await _reportService.GetAllPaging(accountId, request);

            return Ok(result);
        }
    }
}