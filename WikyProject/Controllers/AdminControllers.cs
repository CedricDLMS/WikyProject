using DTOs.ArticleDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DBcontext;
using Models;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using DTOs.UserDTOs;

namespace WikyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminControllers : ControllerBase
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly UserService userService;
        readonly ArticleService articleService;
        

        public AdminControllers(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, UserService _userService, ArticleService _articleService)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.userService = _userService;
            this.articleService = _articleService;
        }

        /// <summary>
        /// Edit all article fields possible
        /// </summary>
        /// <param name="articleId"> Wanted Article </param>
        /// <param name="articleUpdateAdminDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> UpdateArticle(int articleId, ArticleUpdateAdminDTO articleUpdateAdminDTO)
        {
            var result = await articleService.UpdateArticleAdminAsync(articleId, articleUpdateAdminDTO);
            if (!result)
            {
                return NotFound("Article not found or validation failed");
            }

            return Ok("Article updated successfully");
        }


        /// <summary>
        /// Used to manually create a new admin from super admin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Register(UserRegisterDTOs user)
        {
            try
            {
                var result = await userService.CreateAdminAsync(user);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
