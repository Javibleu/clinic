using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            bool userexist = await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username.ToLower());

            if (userexist)                                                                      //if username exist return BadRequest
            {
                return BadRequest("Username is taken");
            }

            AppUser user = _mapper.Map<AppUser>(registerDto);                                   //map registerDto to AppUser

            user.UserName = registerDto.Username.ToLower();                                     //set username to lowercase

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password); //Create User using UserManager

            if (!result.Succeeded) return BadRequest(result.Errors);                            //if not successfull return BadRequest

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Member");      //Add to Role with member as default

            if (!roleResult.Succeeded) return BadRequest(result.Errors);                        //if not successful return BadRequest

            return new UserDto {                                   //Returns UserDto with name, token,known-as and gender
                Username = user.UserName, 
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            AppUser user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false); //login with SignInManager

            if (!result.Succeeded) //if not succeed return Unauthorized
            {
                return Unauthorized();
            }

            return new UserDto      // if success return user + token
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,

            };
        }
    }
}