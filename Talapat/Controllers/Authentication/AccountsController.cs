using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using TalabatCore.Entities.Identity;
using TalabatCore.Services;
using TalabatService;
using Talapat.DTOs;
using Talapat.DTOs.Login;
using Talapat.DTOs.Register;
using Talapat.Errors;
using Talapat.Extentions;

namespace Talapat.Controllers.Authentication
{
    public class AccountsController : BaseAPIController
    {
        public UserManager<AppUser> _UserManager { get; }
        public SignInManager<AppUser> _SignInManager { get; }
        public ITokenService _TokenSerive { get; }
        public IMapper _mapper { get; }

        public AccountsController(UserManager<AppUser> UserManager, SignInManager<AppUser> SignIn,
            ITokenService TokenSerive, IMapper Imapper)
        {
            this._UserManager = UserManager;
            this._SignInManager = SignIn;
            this._TokenSerive = TokenSerive;
            this._mapper = Imapper;
        }


        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegistersDto register)
        {
            if (CheckEmailExist(register.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "Email Already Exist"));
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
                Token = await _TokenSerive.CreateTokenAsync(User,_UserManager),
                Email = User.Email,
            });
        }
        // GetCurrentUser
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Emial = User.FindFirstValue(ClaimTypes.Email);
            var CurrentUser = await _UserManager.FindByEmailAsync(Emial);
            if (CurrentUser is null)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                Email = CurrentUser.Email,
                DisplayName = CurrentUser.DisplayName,
                Token = await _TokenSerive.CreateTokenAsync(CurrentUser, _UserManager)
            });

        }
        //GetCurrentUserAddress
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _UserManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _UserManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            //return Ok(user);
            return (MappedAddress);
        }

        //Update Current User Address
        [Authorize]
        [HttpPost("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto UpdatedAddress)
        {
            var user = await _UserManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
            var result  = await _UserManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return UpdatedAddress;

        }

        // EmailExist
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _UserManager.FindByEmailAsync(email) is not null;
        }

    }
}
