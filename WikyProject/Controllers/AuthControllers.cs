using DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DBcontext;
using Models;
using Repositories;

namespace WikyProject.Controllers
{
    /// <summary>
    /// Controller to override original register
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControllers : ControllerBase
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly UserService userService;
        public AuthControllers(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, UserService _userService)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.userService = _userService;
        }

        /// <summary>
        /// USED TO REGISTER
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Registering")]
        public async Task<IActionResult> Register(UserRegisterDTOs user)
        {
            try
            {
                var result = await userService.CreateUserAsync(user);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

    }
}
