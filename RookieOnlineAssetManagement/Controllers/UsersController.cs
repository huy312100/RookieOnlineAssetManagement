using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Models.Users;
using RookieOnlineAssetManagement.Services;
using RookieOnlineAssetManagement.Utilities.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public UsersController(IUserService userService, SignInManager<User> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest req)
        {
            var result = await _userService.ChangePassword(req);
            if (result == 1)
                return Ok("Password has been changed successfully!");
            else if (result == 2)
                return BadRequest("Incorrect current password!");
            return BadRequest("Fail when change password!");
        }

        [HttpGet("GetUserLogin")]
        public async Task<IActionResult> GetUserIdLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserLogin(User.Identity.Name);

                return Ok(user);
            }
            return BadRequest("Fail to get login data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return BadRequest($"Can not find user with id {id}");
            }
            return Ok(user);
        }

        [HttpGet("paging")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            var accountId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (accountId == null)
            {
                return BadRequest("Permission Denied");
            }
            var result = await _userService.GetAllPaging(accountId, request);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
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
            var userIdCreated = await _userService.Create(accountId, request);
            var userCreated = await _userService.GetUserById(userIdCreated);
            return CreatedAtAction(nameof(GetUserById), new { id = userIdCreated, user = userCreated });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Update(id, request);
            if (result == SystemConstants.APIFindUserByIdErrorInValidId || result == SystemConstants.APIUpdateUserReturnError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("ChangePasswordFirstTime")]
        public async Task<IActionResult> ChangePasswordFirstTime(ChangePasswordFirstTimeModel req)
        {
            var res = await _userService.ChangePasswordFirstTime(req);
            if (!res)
            {
                return BadRequest("Fail to change password first time");
            }
            return Ok("Password changed successfully");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("User logged out.");
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DisableUser(string username)
        {
            try
            {
                var result = await _userService.DisableUser(username);

                if (result == null)
                {
                    return NotFound($"User with Username = {username} not found");
                }
                return Ok("Disable user successfully");
            }
            catch
            {
                return BadRequest($"Username = {username} please close all assignments before disabling user");
            }
        }

        [HttpGet("FindAllAssignmentByUsername/{username}")]
        public async Task<IActionResult> FindAllAssignmentByUsername(string username)
        {
            var result = await _userService.GetAllAssignmentsByUsername(username);
            return Ok(result);
        }
    }
}