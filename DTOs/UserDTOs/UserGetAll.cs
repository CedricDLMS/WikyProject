using DTOs.ArticleDTOs;
using DTOs.CommentDTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.UserDTOs
{
    /// <summary>
    /// Time consuming class, showing everything from every users.
    /// Articles are simplified, and comments. 
    /// </summary>
    public class UserGetAll
    {
        public int Id { get; set; }
        public string Name { get; set; }


        // List articles made by the user
        public List<ArticleListSimpleDTO>? Articles { get; set; }

        // List Comments made by user
        public List<CommentListDTO>? Comments { get; set; }
    }
}
