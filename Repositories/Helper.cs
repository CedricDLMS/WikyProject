using Microsoft.AspNetCore.Identity;
using Models.DBcontext;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class Helper
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;

        public Helper(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }
        /// <summary>
        /// Return true if the specified user id is admin
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="_context"></param>
        /// <param name="_userManager"></param>
        /// <returns></returns>
        public async static Task<bool> isAdmin(int userId,AppDbContext _context, UserManager<AppUser> _userManager)
        {
            var userCurrent = _context.Users.FirstOrDefault(u => u.Id == userId);
            var appUserCurrent = _userManager.Users.FirstOrDefault(u => u.Id == userCurrent.AppUserId);

            var isInRole = await _userManager.IsInRoleAsync(appUserCurrent, "superadmin");

            return isInRole;
        } 
    }
}
