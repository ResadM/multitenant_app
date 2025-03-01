using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using multitenant_app.Models;
using multitenant_app.Services;

namespace multitenant_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly UserDatabaseService _userDatabaseService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService, UserDatabaseService userDatabaseService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userDatabaseService = userDatabaseService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                string databaseName = "MultiTenant_" + model.DatabaseName ?? model.Email.Substring(model.Email.IndexOf("@"));
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    DbName = databaseName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                //Register user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Add user to user role
                    var userRoleResult = await _userManager.AddToRoleAsync(user, EnumUserRoles.User.ToString());
                    if (!userRoleResult.Succeeded)
                    {
                        //Delete user if adding to role failed
                        await _userManager.DeleteAsync(user);
                        return BadRequest();
                    }
                    await _userDatabaseService.CreateUserDatabase(databaseName);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var token = _tokenService.GenerateToken(user);
                        await _userDatabaseService.UpdateDatabase(user.DbName);
                        return Ok(new { Token = token });
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
