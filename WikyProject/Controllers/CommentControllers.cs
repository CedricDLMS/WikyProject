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
    /// <summary>
    /// Controllers for comment editing/creation/deleting/updating
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentControllers : ControllerBase
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly UserService userService;
        readonly CommentService commentService;

        public CommentControllers(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, UserService _userService,CommentService _commentService)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.userService = _userService;
            this.commentService = _commentService;
        }
        /// <summary>
        /// Creation of a commentary, needs Refactos !
        /// </summary>
        /// <param name="content"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost("{content}")]
        [Authorize]

        public async Task<IActionResult> CreateComment(string content, int articleId)
        {
            // NEED REFACTORISATION BUT LATER
            var AppUserID = userManager.GetUserId(User);
            var CurrentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == AppUserID);
            try
            {
                var Comment = new Comment
                {
                    Content = content,
                    UserID = CurrentUser.Id,
                    ArticleID = articleId,
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                };
                await context.AddAsync(Comment);
                await context.SaveChangesAsync();

                var CommentDTO = new CommentListContentDTO()
                {
                    ID = Comment.Id,
                    Created = Comment.Created,
                    LastUpdated = Comment.Updated,
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
        /// <summary>
        /// Edit comment by id
        /// </summary>
        /// <param name="commentEdit"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditComment(CommentEditByUserDTO commentEdit, int id)
        {
            var AppUserID = userManager.GetUserId(User);
            var CurrentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == AppUserID);
            try
            {
                var result = await commentService.UpdateCommentByIdAsync(id, CurrentUser.Id, commentEdit);
                if (!result)
                {
                    return BadRequest("Article not found");
                }
                return Ok(new
                {
                    Message = "Article Updated",
                    Comment = commentEdit.Content
                });
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete comment by ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var appUserID = userManager.GetUserId(User);
            var currentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == appUserID);
            try
            {
                var result = await commentService.DeleteCommentByIdAsync(id, currentUser.Id);
                if (!result)
                {
                    return BadRequest("Comment not found or user not authorized to delete it");
                }
                return Ok($"Comment with id :{id} is now deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get all my comments (of connected user)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserComments()
        {
            // Get the current user's ID from the UserManager
            var appUserID = userManager.GetUserId(User);
            var CurrentUser = await context.Users.FirstOrDefaultAsync(c => c.AppUserId == appUserID);
            if (CurrentUser != null)
            {
                try
                {
                    var comments = await commentService.GetAllCommentsByUserId(CurrentUser.Id);
                    if (comments == null || comments.Count == 0)
                    {
                        return NotFound("No comments found for this user.");
                    }
                    return Ok(comments);
                }
                catch (System.Exception ex)
                {
                    return BadRequest($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
    }
}
