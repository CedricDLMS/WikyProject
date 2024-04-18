using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DBcontext;
using Models;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using DTOs.ArticleDTOs;
using Microsoft.EntityFrameworkCore;

namespace WikyProject.Controllers
{
    /// <summary>
    /// Controllers for articles editing/Creating/Reading/Updating
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleControllers : ControllerBase
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly UserService userService;
        readonly ArticleService articleService;

        public ArticleControllers(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, UserService _userService, ArticleService _articleService)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.userService = _userService;
            this.articleService = _articleService;
        }

        /// <summary>
        /// Get all article without comments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var list = await articleService.ListArticlesAsync();
            return Ok(list);
        }

        /// <summary>
        /// Read all articles with comments 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ReadAllArticleWithComment()
        {
            var list = await articleService.ListArticlesWithCommentsAsync();
            return Ok(list);
        }
        /// <summary>
        /// Reading an article with the comments section
        /// </summary>
        /// <param name="id">id of the article</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ReadArticle(int id)
        {
            var article = await articleService.ReadByIdAsync(id);
            return Ok(article);
        }
        /// <summary>
        /// Update the Article with DTO and Id of article in input, admin has a special update tool controller , so no access to this one 
        /// </summary>
        /// <param name="article"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(ArticleUpdateDTO article, int id)
        {
            var AppUserID = userManager.GetUserId(User);
            var CurrentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == AppUserID);
            try
            {
                var edit = await articleService.UpdateArticleAsync(id,CurrentUser.Id,article);
                if (!edit)
                {
                    return BadRequest("Article Might not exist or No permission to edit");
                }
                return Ok(article);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Create an article based on the DTO
        /// </summary>
        /// <param name="article"></param>
        /// <returns>Hope everythings allright</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> CreateArticleAsync(ArticleCreateDTO article)
        {
            var AppUserID = userManager.GetUserId(User);
            var CurrentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == AppUserID);
            var Article = new Article
            {
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                Content = article.Content,
                Title = article.Title,
                UserID = CurrentUser.Id,
                Priority = article.Priority,
                ThemeID = article.ThemeID,
            };

            await context.AddAsync(Article);

            var a = await context.Themes.ToListAsync();

            var ArticleGet = new ArticleGetDTO
            {
                Id = Article.Id,
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                Content = article.Content,
                Title = article.Title,
                Priority = article.Priority,
                Theme = a.FirstOrDefault(a => a.Id == Article.ThemeID).Name
            };

            await context.SaveChangesAsync();
            return Ok(ArticleGet);
        }

        /// <summary>
        /// Remove the specified article and associated comments , admin can use for all
        /// </summary>
        /// <param name="articleId">The article ID to delet</param>
        /// <returns>Might return exceptions if not found or no authorisation</returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveArticle(int articleId)
        {
            var AppUserID = userManager.GetUserId(User);
            var CurrentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == AppUserID);

            try
            {
                var remove = await articleService.RemoveArticleAsync(articleId, CurrentUser.Id);
                if (!remove)
                {
                    return BadRequest("Article Might not exist");
                }
                return Ok($"Article With Id : {articleId} is now removed ");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesByTheme(int themeId)
        {
            var themeArticlesDto = await articleService.GetArticlesByThemeIdAsync(themeId);
            if (themeArticlesDto == null)
            {
                return NotFound();
            }
            return Ok(themeArticlesDto);
        }
    }
}
