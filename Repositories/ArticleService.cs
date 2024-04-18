using Microsoft.AspNetCore.Identity;
using Models.DBcontext;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.ArticleDTOs;
using Microsoft.EntityFrameworkCore;
using DTOs.CommentDTOs;
using DTOs.Theme;

namespace Repositories
{
    /// <summary>
    /// Tools for Article
    /// </summary>
    public class ArticleService
    {
        readonly AppDbContext context;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        public ArticleService(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }
        /// <summary>
        /// Used to get a list of articles with useful informations
        /// </summary>
        /// <returns>Return a list of articles without comments</returns>
        public async Task<List<ArticleListDTO>> ListArticlesAsync()
        {

            List<Article> articlesList = await context.Articles
                .Include(a => a.Theme)
                .Include(u => u.User)
                .ToListAsync();

            List<ArticleListDTO> dtos = articlesList.Select(article => new ArticleListDTO
            {
                Id = article.Id,
                AuthorName = article.User.Name,
                CreationDate = article.CreationDate,
                EditDate = article.EditDate,
                Title = article.Title,
                Theme = article.Theme.Name,
                Priority = article.Priority
            }).OrderBy(c=> c.Priority).ThenBy(i=>i.EditDate).ToList();

            return dtos;
        }
        /// <summary>
        /// List all articles with comments , could be used by admin ? 
        /// </summary>
        /// <returns>List of article</returns>
        public async Task<List<ArticleReadWithComDTO>> ListArticlesWithCommentsAsync()
        {
            // Doing all includes for right requests
            var articlesList = await context.Articles
                .Include(a => a.Comments)
                .ThenInclude(c => c.User)
                .Include(a => a.User)
                .Include(a => a.Theme)
                .ToListAsync();

            List<ArticleReadWithComDTO> dtos = articlesList.Select(article => new ArticleReadWithComDTO
            {
                CreationDate = article.CreationDate,
                EditDate = article.EditDate,
                AuthorName = article.User?.Name,
                Title = article.Title,
                Content = article.Content,
                Theme = article.Theme.Name,
                Priority = article.Priority,
                Comments = article.Comments?.Select(c => new CommentListContentDTO
                {
                    ID = c.Id,  // Showing all datas here, so an admin can get id to update a wrong comment
                    authorName = c.User.Name,
                    content = c.Content,
                    Created = c.Created,
                    LastUpdated = c.Updated,
                }).ToList()
            }).ToList();

            return dtos;
        }
        /// <summary>
        /// Read Article By Id
        /// </summary>
        /// <param name="articleId">The wanted article ID</param>
        /// <returns>Return the specified article DTO with Comments</returns>
        public async Task<ArticleReadWithComDTO> ReadByIdAsync(int articleId)
        {
            var article = await context.Articles
                .Include(a => a.Comments)
                .ThenInclude(c => c.User)
                .Include(a => a.User)    // Only if there's an Author object and you need data from it
                .Include(a => a.Theme)     // Include this if Theme is a navigation property
                .FirstOrDefaultAsync(a => a.Id == articleId);

            if (article == null)
            {
                return null;
            }
            var dto = new ArticleReadWithComDTO
            {
                CreationDate = article.CreationDate,
                EditDate = article.EditDate,
                AuthorName = article.User?.Name, // Check if Author is not null
                Title = article.Title,
                Content = article.Content,
                Theme = article.Theme?.Name, // Check if Theme is not null
                Priority = article.Priority,
                Comments = article.Comments?.Select(comment => new CommentListContentDTO
                {
                    ID = comment.Id,
                    authorName = comment.User.Name,
                    content = comment.Content,
                    Created = comment.Created,
                    LastUpdated = comment.Updated,
                }).ToList()
            };

            return dto;
        }
        /// <summary>
        /// Method to edit an article and keep track of the edit date
        /// </summary>
        /// <param name="articleId">Article to be edited id</param>
        /// <param name="userId">Current User ID checker</param>
        /// <param name="articleUpdateDTO">DTO for the editing </param>
        /// <returns> Returns true if done rightly, return Exception if User Creator and Current user doesn't match </returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<bool> UpdateArticleAsync(int articleId, int userId, ArticleUpdateDTO articleUpdateDTO)
        {
            var article = await context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return false; // Article not found
            }

            // Check if the current user is the creator of the article
            if (article.UserID != userId)
            {
                throw new Exception("You do not have permission to edit this article.");
            }

            // Update the article properties
            article.Title = articleUpdateDTO.Title;
            article.Content = articleUpdateDTO.Content;
            article.ThemeID = articleUpdateDTO.ThemeID;
            article.EditDate = DateTime.Now;

            // Save changes in the database
            context.Articles.Update(article);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateArticleAdminAsync(int articleId, ArticleUpdateAdminDTO articleUpdateAdminDTO)
        {
            var article = await context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return false; // Article not found
            }
            if(articleUpdateAdminDTO.ThemeID == null || (int)articleUpdateAdminDTO.Priority > 2 || (int)articleUpdateAdminDTO.Priority < 0 ) // Check if inputs are good
            {
                return false;
            }
            // Update the article properties
            article.Title = articleUpdateAdminDTO.Title;
            article.Content = articleUpdateAdminDTO.Content;
            article.ThemeID = articleUpdateAdminDTO.ThemeID;
            article.EditDate = DateTime.Now;
            article.Priority = articleUpdateAdminDTO.Priority;

            // Save changes in the database
            context.Articles.Update(article);
            await context.SaveChangesAsync();

            return true;
        }
        /// <summary>
        /// Remove an article by id, checking if the user have rights for it
        /// </summary>
        /// <param name="articleId">Article id to remove</param>
        /// <param name="userId">Current user id</param>
        /// <returns>Returns a bool if removing went right</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<bool> RemoveArticleAsync(int articleId, int userId)
        {
            // Attempt to retrieve the article from the database
            var article = await context.Articles.FindAsync(articleId);
            var comments = context.Comments.Where(c=>c.ArticleID == articleId).ToList();


            if (article == null)
            {
                return false; // Article not found
            }

            // Check if the current user is the creator of the article
            if (article.UserID != userId)
            {
                if (await Helper.isAdmin(userId, context, userManager)) // check if is admin
                {
                    context.Articles.Remove(article);

                    foreach (var comment in comments)
                    {
                        context.Remove(comment);
                    }

                    await context.SaveChangesAsync();

                    return true; // return true and exit scope
                }
                throw new Exception("You do not have permission to delete this article.");
            }

            // Remove the article from the database
            context.Articles.Remove(article);

            foreach (var comment in comments)
            {
                context.Remove(comment);
            }
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Return list of article from specified theme
        /// </summary>
        /// <param name="themeId"></param>
        /// <returns></returns>
        public async Task<ThemeGetArticleDTO?> GetArticlesByThemeIdAsync(int themeId)
        {
            // Assuming _dbContext is your EF database context
            var themeWithArticles = await context.Themes
                .Where(t => t.Id == themeId)
                .Select(t => new ThemeGetArticleDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Articles = t.Articles.Select(a => new ArticleListSimpleDTO
                    {
                        ID = a.Id,
                        CreationDate = a.CreationDate,
                        Title = a.Title,
                        Theme = t.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            return themeWithArticles;
        }
    }
}
