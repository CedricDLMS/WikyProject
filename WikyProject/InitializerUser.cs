using Microsoft.AspNetCore.Identity;
using Models.DBcontext;
using Models;

namespace WikyProject
{
    public class InitializerUser
    {
        //readonly AppDbContext context;
        //readonly UserManager<AppUser> userManager;
        //readonly SignInManager<AppUser> signInManager;

        //public InitializerUser(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        //{
        //    this.context = _context;
        //    this.userManager = _userManager;
        //    this.signInManager = _signInManager;
        //}

        public async static Task UserAndRolesInit(AppDbContext context, UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.FindByNameAsync("SUPERADMIN") == null)
            {
                string adminPassword = "Azerty1+";
                var userAdmin = new AppUser { Email = "admin@admin.fr", NormalizedEmail = "admin@admin.fr", UserName = "admin@admin.fr" };
                var roleAdmin = new IdentityRole { Name = "superadmin", NormalizedName = "SUPERADMIN" };

                var userClass = new User { AppUserId = userAdmin.Id, Name = "admin", AppUser = userAdmin };

                var roleAdminCheck = await context.Roles.FindAsync(roleAdmin.Id);
                var userAdminCheck = await context.Users.FindAsync(userClass.Id);

                if (roleAdminCheck == null && userAdminCheck == null)
                {
                    await userManager.CreateAsync(userAdmin, adminPassword);
                    await roleManager.CreateAsync(roleAdmin);

                    await userManager.AddToRoleAsync(userAdmin, "SUPERADMIN");

                    await context.AddAsync(userClass);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
