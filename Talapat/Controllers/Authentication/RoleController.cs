using AutoMapper;
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
    public class RoleController : BaseAPIController
    {
        public RoleManager<AppRole> _RoleManager { get; }
        
        public IMapper _Mapper { get; }

        public RoleController(RoleManager<AppRole> RoleManager, IMapper Mapper)
        {
            this._RoleManager = RoleManager;
            this._Mapper = Mapper;
        }


        // GET: AllRoles
        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<List<AppRole>>> GetAllRoles()
        {
            var Roles = await _RoleManager.Roles.ToListAsync();
            if(Roles == null)
                return NotFound(new ApiResponse(404));
            return Ok(Roles);
        }
        // Create Role
        [HttpPost("CreateRole")]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] RoleDto role)
        {
            // role is null or name of role is null
            if (role == null || role.Name is null)
                return BadRequest(new ApiResponse(400, "Role Name is Required"));

            // role exists
            var roleExist = await _RoleManager.RoleExistsAsync(role.Name);
            if (roleExist)
                return BadRequest(new ApiResponse(400, "Role Already Exist"));

            // Create Role
            var roleToCreate = new AppRole
            {
                Name = role.Name,
                NormalizedName = role.Name.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var result = await _RoleManager.CreateAsync(roleToCreate);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed To Create Role"));
            return Ok(new RoleDto()
            {
                Id = roleToCreate.Id,
                Name = roleToCreate.Name
            }); 
        }
        // UpdateRole
        [HttpPost("UpdateRole")]
        public async Task<ActionResult<RoleDto>> UpdateRole([FromBody] RoleDto role)
        {
            //if role is null or name of role is null
            if (role == null || string.IsNullOrEmpty(role.Name))
            {
                return BadRequest(new ApiResponse(400, "Role name is required."));
            }

            // If Role Exists
            var existingRole = await _RoleManager.FindByIdAsync(role.Id);
            if (existingRole is null)
            {
                return NotFound(new ApiResponse(404, "Role not found."));
            }

            // Update Role
            
            _Mapper.Map<RoleDto,AppRole>(role, existingRole); 
            var result = await _RoleManager.UpdateAsync(existingRole);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to update role."));
            }

            return Ok(new RoleDto()
            {
                Id = existingRole.Id,
                Name = existingRole.Name
            });
        }

        // DeleteRole
        [HttpDelete("DeleteRole")]
        public async Task<ActionResult<bool>> DeleteRole([FromBody] RoleDto role)
        {
            if (role == null || string.IsNullOrEmpty(role.Name))
            {
                return BadRequest(new ApiResponse(400, "Role name is required."));
            }
            // If Role Exists
            var existingRole = await _RoleManager.FindByIdAsync(role.Id);
            if (existingRole == null)
            {
                return NotFound(new ApiResponse(404, "Role not found."));
            }

            var result = await _RoleManager.DeleteAsync(existingRole);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to delete role."));
            }

            return Ok(true);
        }

    }
}
