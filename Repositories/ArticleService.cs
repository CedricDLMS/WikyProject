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
        /// Used to get all articles
        /// </summary>
        /// <returns>Return a list of articles without comments</returns>
        public async Task<List<ArticleListDTO>> ListArticlesAsync()
        {
           
            List<Article> articlesList = await context.Articles
                .Include(a=>a.Theme)
                .ToListAsync();

            List<ArticleListDTO> dtos = articlesList.Select(article => new ArticleListDTO
            {
                Id = article.Id,
                CreationDate = article.CreationDate,
                EditDate = article.EditDate,
                Title = article.Title,
                Theme = article.Theme.Name,  
                Priority = article.Priority
            }).ToList();

            return dtos;
        }

        public async Task<List<ArticleReadWithComDTO>> ListArticlesWithCommentsAsync()
        {
            // Assuming _dbContext is your database context
            var articlesList = await context.Articles
                .Include(a => a.Comments)  // Ensure to include Comments
                .Include(a => a.User)    // Assuming there is an Author navigation property
                .Include(a => a.Theme)     // Include Theme if it's a navigation property
                .ToListAsync();

            List<ArticleReadWithComDTO> dtos = articlesList.Select(article => new ArticleReadWithComDTO
            {
                CreationDate = article.CreationDate,
                EditDate = article.EditDate,
                AuthorName = article.User?.Name,  // Assuming Author has a Name property
                Title = article.Title,
                Content = article.Content,
                Theme = article.Theme.Name,  // Assuming Theme is an object with a Name property
                Priority = article.Priority,
                Comments = article.Comments?.Select(c => new CommentListContentDTO
                {
                    content = c.Content
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
                .ThenInclude(c=> c.User)
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
            if (article == null)
            {
                return false; // Article not found
            }

            // Check if the current user is the creator of the article
            if (article.UserID != userId)
            {
                throw new Exception("You do not have permission to delete this article.");
            }

            // Remove the article from the database
            context.Articles.Remove(article);
            await context.SaveChangesAsync();

            return true;
        }


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
