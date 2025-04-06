using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalabatCore.Entities.Identity;
using TalabatCore.Services;
using Talapat.DTOs.Login;
using Talapat.DTOs.Register;
using Talapat.Errors;

namespace Talapat.Controllers.Authentication
{

    public class AdminController : BaseAPIController
    {
        public AdminController(SignInManager<AppUser> signIn, UserManager<AppUser> userManager, ITokenService TokenService)
        {
            _SignIn = signIn;
            _UserManager = userManager;
            this._TokenService = TokenService;
        }

        public SignInManager<AppUser> _SignIn { get; }
        public UserManager<AppUser> _UserManager { get; }
        public ITokenService _TokenService { get; }

        // login
        [HttpPost("Login")]
        
        public async Task<ActionResult<UserDto>> Login(AdminLoginDto login)
        {
            var user = await _UserManager.FindByEmailAsync(login.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "Invalid Email"));
            var result = await _SignIn.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401, "Invalid Password"));
            var roles = await _UserManager.IsInRoleAsync(user, "Admin");
            if (!roles)
                return Unauthorized(new ApiResponse(401, "You Are Not Admin"));
            // Token
            var token = await _TokenService.CreateTokenAsync(user, _UserManager);

            return Ok(new AdminLoginResponseDto()
            {
                DisplayName = user.DisplayName,
                Token = token,
                Email = user.Email,
            });
        }

        // Logout
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            // Just a placeholder - JWT logout usually happens client-side by deleting token
            return Ok(new { Message = "Logged out successfully. Remove the token from the client." });
        }

    }
}
