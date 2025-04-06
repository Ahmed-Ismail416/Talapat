using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalabatCore.Entities.Identity;
using Talapat.DTOs.Role;
using Talapat.Errors;

namespace Talapat.Controllers.Authentication
{
    public class UserController : BaseAPIController
    {
        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        public UserManager<AppUser> _UserManager { get; }
        public RoleManager<AppRole> _RoleManager { get; }

        // Get All Users
        [HttpGet("GetAllUsersWithRules")]
        public async Task<ActionResult<IReadOnlyList<UserWithRoleDto>>> GetAllUsersWithRules()
        {
            var users = await _UserManager.Users.Select(x => new UserWithRoleDto()
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Roles = _UserManager.GetRolesAsync(x).Result
            }).ToListAsync();
            return Ok(users);
        }
        // Edit User
        [HttpPost("EditUserRole")]
        public async Task<ActionResult<UserWithRoleDto>> EditUserRole(RoleOfUserDto dto)
        {
            var User = await _UserManager.FindByIdAsync(dto.UserId);
            if (User is null)
                return NotFound(new ApiResponse(404, "Not Fount The User"));

            var CurrentRoles = await _UserManager.GetRolesAsync(User);

            if (CurrentRoles.Count() > 0)
            {
                // Remove Old Roles

                var RemoveResult = await _UserManager.RemoveFromRolesAsync(User, CurrentRoles);

                if (!RemoveResult.Succeeded)
                    return BadRequest(new ApiResponse(400, "Failed To Remove Current Roles"));
            }
            // Add New Role

            var AddResult = await _UserManager.AddToRolesAsync(User, dto.RolesNames);
            if (!AddResult.Succeeded)
                return BadRequest(new ApiResponse(400));


            return Ok(new UserWithRoleDto()
            {
                Id = User.Id,
                UserName = User.UserName,
                Email = User.Email,
                Roles = await _UserManager.GetRolesAsync(User)
            });

        }
    }
}
