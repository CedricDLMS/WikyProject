using DTOs.ArticleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Theme
{
    /// <summary>
    /// Used when finding article with theme , so data is clearer
    /// </summary>
    public class ThemeGetArticleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ArticleListSimpleDTO>? Articles { get; set; }
    }
}
