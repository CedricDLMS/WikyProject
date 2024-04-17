using DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DBcontext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserService
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        public UserService(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }
        /// <summary>
        /// Creating an user in all tables , app user and user simple, using a DTO for prettier front
        /// </summary>
        /// <param name="userNew">Give an User DTO so the Front is prettier</param>
        /// <returns>Return the userDTO when task completed</returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserGetAll> CreateUserAsync(UserRegisterDTOs userNew)
        {
            
            EmailAddressAttribute emailAddress = new EmailAddressAttribute();
            PasswordValidator<AppUser> passwordValidator = new PasswordValidator<AppUser>();

            var AppUser = new AppUser
            {
                UserName = userNew.Mail,
                Email = userNew.Mail,
            };

            if (!emailAddress.IsValid(userNew.Mail))
            {
                throw new Exception(emailAddress.ErrorMessage);
            }
            else if (userNew.Password1 != userNew.Password2)
            {
                throw new Exception($"Differents passwords");
            }

            var validate = await passwordValidator.ValidateAsync(userManager, AppUser, userNew.Password1);

            if (!validate.Succeeded)
            {
                throw new Exception("Password Not valid format, need 1 upper case and one number at least");
            }
            if (await CheckBirthAgeAsync(userNew.BirthDate) == false)
            {
                throw new Exception("Must Be over 18 To register");
            }



            var UserC = new User { AppUser = AppUser, Name = userNew.Name, AppUserId = AppUser.Id };

            var UserY = new UserGetAll { Id = UserC.Id, Name = UserC.Name, AppUserId = AppUser.Id };
            var result = await userManager.CreateAsync(AppUser, userNew.Password1);
            var c = await context.AddAsync(UserC);
            var y = await context.SaveChangesAsync();

            return await Task.FromResult(UserY);


        }
        /// <summary>
        /// Does time diff for users
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        public async Task<bool> CheckBirthAgeAsync(DateTime birthDate)
        {
           
            var DateNow = DateTime.Now;
            TimeSpan timeSpan = DateTime.Now - birthDate;
            var differenceInYears = timeSpan.Days / 365;

            if (differenceInYears <= 18)
            {
                return false;
            }
            return true;
        }

    }
}
