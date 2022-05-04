using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Models.ReturnAssets;
using RookieOnlineAssetManagement.Services;
using RookieOnlineAssetManagement.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ReturnAssetsController: ControllerBase
    {
        private readonly IReturnAssetService _returnAssetService;

        public ReturnAssetsController(IReturnAssetService returnAssetService)
        {
            _returnAssetService = returnAssetService;
        }

        [HttpGet("paging")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetReturnAssetPagingRequest request)
        {
            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }
            var result = await _returnAssetService.GetAllPaging(accountId, request);

            return Ok(result);
        }


        [HttpPost("CreateRequestReturnAssetUser/{id}")]
        public async Task<IActionResult> CreateRequestReturnAssetUser(int id)
        {
            var result = await _returnAssetService.CreateRequestReturnAssetUser(id);
            if (result == null)
            {
                return BadRequest($" Can not find assignment Id = {id} ");
            }
            return Ok("Request return asset for user successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin/{assignmentId}")]
        public async Task<IActionResult> AdminCreate(int assignmentId)
        {
            var result = await _returnAssetService.CreateRequestReturnAssetUser(assignmentId);
            if (result == null)
            {
                return BadRequest(SystemConstants.APIFindAssignmentByIdErrorInValidId);
            }
            return Ok(SystemConstants.APICreateReturnAssetSuccess);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("AcceptRequestReturnAsset/{id}")]
        public async Task<IActionResult> AcceptRequestReturnAsset(int id)
        {
            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            var result = await _returnAssetService.AcceptRequestReturnAsset(accountId, id);
            if (result == null)
            {
                return BadRequest($" Can not find return asset Id = {id} ");
            }
            return Ok("Accept request return asset successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("CancelRequestReturnAsset/{id}")]
        public async Task<IActionResult> CancelRequestReturnAsset(int id)
        {
            var result = await _returnAssetService.CancelRequestReturnAsset(id);
            if (result == null)
            {
                return BadRequest($" Can not find return asset Id = {id} ");
            }
            return Ok("Cancel request return asset successfully");
        }
    }
}
