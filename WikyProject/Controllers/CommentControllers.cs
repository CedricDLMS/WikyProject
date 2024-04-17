using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DBcontext;
using Models;

using Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DTOs.CommentDTOs;

namespace WikyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentControllers : ControllerBase
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly UserService userService;

        public CommentControllers(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, UserService _userService)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.userService = _userService;
        }

        [HttpPost("{content}")]
        [Authorize]

        public async Task<IActionResult> CreateComment(string content, int articleId)
        {
            var AppUserID = userManager.GetUserId(User);
            var CurrentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == AppUserID);
            try
            {
                var Comment = new Comment
                {
                    Content = content,
                    UserID = CurrentUser.Id,
                    ArticleID = articleId,
                    Created = DateTime.Now
                };
                await context.AddAsync(Comment);
                await context.SaveChangesAsync();

                var CommentDTO = new CommentListContentDTO()
                {
                    ID = Comment.Id,
                    Created = Comment.Created,
                    content = Comment.Content,
                    authorName = context.Users.FirstOrDefault(c => c.Id == Comment.UserID).Name,

                };

                return Ok(CommentDTO);
            }
            catch (Exception ex)
            {
                return BadRequest("Something Went Wrong oops");
            }

        }

    }
}
