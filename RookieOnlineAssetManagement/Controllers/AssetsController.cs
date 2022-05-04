using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Models.Assets;
using RookieOnlineAssetManagement.Models.Common;
using RookieOnlineAssetManagement.Services;
using RookieOnlineAssetManagement.Utilities.Constants;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet("paging")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetAssetPagingRequest request)
        {
            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }
            var result = await _assetService.GetAllPaging(accountId, request);

            return Ok(result);
        }

        [HttpGet("{id}/history")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAssignHistoryPaging(int id, [FromQuery] PagingRequestBase request)
        {
            var result = await _assetService.GetAssetHistoryPaging(id, request);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _assetService.GetById(id);
            if (asset == null)
            {
                return BadRequest($"Can not find asset with id {id}");
            }
            return Ok(asset);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAssetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }

            var assetIdCreated = await _assetService.Create(accountId, request);
            if (assetIdCreated == -1)
            {
                return BadRequest("Permission Denied");
            }
            if (assetIdCreated == -2)
            {
                return BadRequest($"Can not find category with id {request.CategoryId}");
            }
            if (assetIdCreated == 0)
            {
                return BadRequest("Create asset failed");
            }
            var assetCreated = await _assetService.GetById(assetIdCreated);
            return CreatedAtAction(nameof(GetById), new { id = assetIdCreated, asset = assetCreated });
        }


        [HttpGet("CheckAssetHistory/{assetId}")]
        public async Task<IActionResult> CheckAssetHistory(int assetId)
        {
            var result = await _assetService.CheckAssetHistory(assetId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromForm] UpdateAssetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest(SystemConstants.APIReturnPermissionDenied);
            }

            var result = await _assetService.Update(id, request);
            if(result == SystemConstants.APIFindAssetByIdErrorInValidId || result == SystemConstants.APIUpdateAssetReturnError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            try
            {
                var check = await _assetService.CheckAssetHistory(id);
                if (check == true)
                {
                    return BadRequest($"assetId = {id} please update asset state");
                }
                else
                {
                    var result = await _assetService.DeleteAsset(id);
                    if (result == null)
                    {
                        return NotFound($"Asset with Asset Id = {id} not found");
                    }
                    return Ok("Delete Asset successfully");
                }
            }
            catch
            {
                return BadRequest($"assetId = {id} please update asset state");
            }
        }
    }
}