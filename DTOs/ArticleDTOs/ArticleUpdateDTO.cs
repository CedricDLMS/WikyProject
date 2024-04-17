using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Data that can be updated by user
    /// </summary>
    public class ArticleUpdateDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? ThemeID { get; set; }
        
    }
}
