using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DBcontext;
using Models;
using Repositories;
using DTOs.Theme;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WikyProject.Controllers
{
    /// <summary>
    /// Controllers for theme editing/updating/creating/deleting
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeControllers : ControllerBase
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly UserService userService;
        readonly ThemeService themeService;
        public ThemeControllers(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, UserService _userService, ThemeService _themeService)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.userService = _userService;
            this.themeService = _themeService;
        }
        /// <summary>
        /// Theme Creation only for admins
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "superadmin")]
        
        public async Task<IActionResult> CreateTheme(ThemeSimpleDTO theme)
        {
            try
            {
                var result = await themeService.CreateTheme(theme);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
