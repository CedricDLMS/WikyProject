using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Same as GetDTO , just without the ID , not used there 
    /// </summary>
    public class ArticleReadDTO
    {
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string? AuthorName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Theme { get; set; }
        public Priority Priority { get; set; }
    }
}
