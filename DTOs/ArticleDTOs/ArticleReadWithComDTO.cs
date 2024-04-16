using DTOs.CommentDTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Reading the article with the comments
    /// </summary>
    public class ArticleReadWithComDTO
    {
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string? AuthorName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Theme { get; set; }
        public Priority Priority { get; set; }

        public List<CommentListContentDTO>? Comments { get; set; }
    }
}
