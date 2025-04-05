using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalabatCore.Entities.Identity;
using TalabatCore.Services;
using TalabatService;
using Talapat.DTOs.Login;
using Talapat.DTOs.Register;
using Talapat.Errors;

namespace Talapat.Controllers.Authentication
{
    public class AccountsController : BaseAPIController
    {
        public UserManager<AppUser> _UserManager { get; }
        public SignInManager<AppUser> _SignInManager { get; }
        public ITokenService _TokenSerive { get; }

        public AccountsController(UserManager<AppUser> UserManager, SignInManager<AppUser> SignIn, ITokenService TokenSerive)
        {
            this._UserManager = UserManager;
            this._SignInManager = SignIn;
            this._TokenSerive = TokenSerive;
        }


        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegistersDto register)
        {
            var User = new AppUser()
            {
                DisplayName = register.DisplayName,
                Email       = register.Email,
                UserName    = register.Email,
                PhoneNumber = register.PhoneNumber
            };
            var result = await _UserManager.CreateAsync(User, register.Password);
            if(!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Token = await _TokenSerive.CreateTokenAsync(User, _UserManager),
                Email = User.Email,
                
            });
        }

        //login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var User = await _UserManager.FindByEmailAsync(login.Email);
            if (User is null)
                return Unauthorized(new ApiResponse(401));

            var Result = await _SignInManager.CheckPasswordSignInAsync(User, login.Password, false);
            if(!Result.Succeeded)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Token = "fds",
                Email = User.Email,
            });
        }
    }
}
