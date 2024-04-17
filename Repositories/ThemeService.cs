using DTOs.Theme;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DBcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    /// <summary>
    /// Tools for Themes
    /// </summary>
    public class ThemeService
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        public ThemeService(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }

        public async Task<ThemeListDTO> CreateTheme(ThemeSimpleDTO theme)
        {

            if (theme.Name.Length > 15)
            {
                throw new Exception("Theme can't exceed 15 characters");
            }
            else if (theme.Name.IsNullOrEmpty())
            {
                throw new Exception("Theme can't be empty");
            }
            else
            {
                var newTheme = new Theme
                {
                    Name = theme.Name
                };

                // add to DB if valid
                await context.AddAsync(newTheme);
                await context.SaveChangesAsync();
                var themeDTO = new ThemeListDTO
                {
                    Id = newTheme.Id,
                    Name = newTheme.Name,
                };
                return await Task.FromResult(themeDTO);
            }
        }
    }
}
