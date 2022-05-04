using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Models.Assignments;
using RookieOnlineAssetManagement.Services;
using RookieOnlineAssetManagement.Utilities.Constants;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var asset = _assignmentService.GetById(id);
            if (asset == null)
            {
                return BadRequest($"Can not find assignment with id {id}");
            }
            return Ok(asset);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAssignmentRequest request)
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

            var assignmentIdCreated = await _assignmentService.Create(accountId, request);
            if (assignmentIdCreated == -3)
            {
                return BadRequest($"Can not find asset with id {request.AssetId}");
            }
            if (assignmentIdCreated == -2)
            {
                return BadRequest($"Can not find user with id {request.UserId}");
            }
            if (assignmentIdCreated == -1)
            {
                return BadRequest("Permission Denied");
            }
            if (assignmentIdCreated == 0)
            {
                return BadRequest("Create asset failed");
            }
            var assignmentCreated = _assignmentService.GetById(assignmentIdCreated);
            return CreatedAtAction(nameof(GetById), new { id = assignmentIdCreated, assignment = assignmentCreated });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetAssignmentPagingRequest request)
        {
            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }
            var result = await _assignmentService.GetAllPaging(accountId, request);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }
            var result = await _assignmentService.DeleteAssignment(accountId, id);
            if (result == null)
            {
                return NotFound($"Assignment with Assignment Id = {id} not found");
            }
            return Ok("Delete Assignment successfully");
        }

        [HttpGet("HomePaging")]
        public async Task<IActionResult> GetHomePaging([FromQuery] GetHomePagingRequest request)
        {
            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }
            var result = await _assignmentService.GetHomePaging(accountId, request);

            return Ok(result);
        }

        [HttpPut("ChangeAssignmentState/{id}")]
        public async Task<IActionResult> ChangeAssignmentState(int id)
        {
            var result = await _assignmentService.ChangeAssignmentState(id);
            if (result == null)
            {
                return BadRequest($" Can not find assignment Id = {id} ");
            }
            return Ok("Change assignment state successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromForm] UpdateAssignmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _assignmentService.Update(id, request);
            if (result == SystemConstants.APIFindAssignmentByIdErrorInValidId
                || result == SystemConstants.APIFindUserByIdErrorInValidId
                || result == SystemConstants.APIFindAssetByIdErrorInValidId
                || result == SystemConstants.APIUpdateAssignmentReturnError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}