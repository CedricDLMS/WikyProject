using Microsoft.AspNetCore.Identity;
using Models.DBcontext;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.CommentDTOs;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    /// <summary>
    /// Tools for comment managing
    /// </summary>
    public class CommentService
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        
        public CommentService(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }
        /// <summary>
        /// Method to update an arcle, will be used in comm controllers
        /// </summary>
        /// <param name="commentID">Id of the comment to be updated</param>
        /// <param name="userID">ID of the user trying to update</param>
        /// <returns>Returns True if editing went well, false or exception if not</returns>
        public async Task<bool> UpdateCommentByIdAsync(int commentID,int userID, CommentEditByUserDTO commentEdit)
        {
            var comment = await context.Comments.FindAsync(commentID);
            if(comment == null)
            {
                return false;
            }
            if(userID != comment.UserID)
            {
                throw new Exception("You're not the author of this comment");
            }
            
            // EDITING IF EVERYTHING GOOD
            comment.Content = commentEdit.Content;
            comment.Updated = DateTime.Now;
            
            context.Comments.Update(comment);
            await context.SaveChangesAsync();

            return true;

        }

        /// <summary>
        /// User to delet Comment by ID
        /// </summary>
        /// <param name="commentId">comment Id to remove</param>
        /// <param name="userId">Current user ID </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> DeleteCommentByIdAsync(int commentId, int userId)
        {
            var comment = await context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return false; // Comment not found
            }

            if (userId != comment.UserID)
            {
                throw new Exception("You're not the author of this comment"); 
            }

            // Deleting the comment if the user is the author
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return true; 
        }

        /// <summary>
        /// Get all Comments Made By an user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CommentFindByUserDTO>> GetAllCommentsByUserId(int userId)
        {
            var comments = await context.Comments
                .Where(c => c.UserID == userId)  // Filter comments by user ID
                .Select(c => new CommentFindByUserDTO
                {
                    Id = c.Id,
                    ArticleID = c.ArticleID,
                    LastUpdate = c.Updated,
                    ArticleTitle = c.Article.Title, // Assuming the navigation property from comment to article is named `Article`
                    Content = c.Content
                }).OrderByDescending(d => d.LastUpdate).ToListAsync();

            return comments;
        }
    }
}
