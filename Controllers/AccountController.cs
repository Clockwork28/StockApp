using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockApp.DTOs.Account;
using StockApp.Models;

namespace StockApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController :ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
                    if (roleAssigned.Succeeded) return Ok("User created");
                    else return StatusCode(400, roleAssigned.Errors);
                }
                else return StatusCode(400, userCreated.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
