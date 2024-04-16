using DTOs.ArticleDTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.UserDTOs
{
    /// <summary>
    /// Data showed when geting one user
    /// </summary>
    public class UserGetArticleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ArticleListSimpleDTO> ArticleList { get; set; }
        
    }
}
