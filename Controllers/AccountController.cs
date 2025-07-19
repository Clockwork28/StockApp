using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApp.DTOs.Account;
using StockApp.Interfaces;
using StockApp.Models;

namespace StockApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController :ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var appUser = new AppUser
                {
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email,
                };
                var userCreated = await _userManager.CreateAsync(appUser, registerDTO.Password);
                if (userCreated.Succeeded)
                {
                    var roleAssigned = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleAssigned.Succeeded) return Ok(
                        new NewUserDTO
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)   
                        }
                    );
                    else return StatusCode(400, roleAssigned.Errors);
                }
                else return StatusCode(400, userCreated.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.Username);
            if (user == null) return Unauthorized("Username or Password is invalid");
            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!loginResult.Succeeded) return Unauthorized("Username or Password is invalid");
            return Ok(new NewUserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }
    }
}
