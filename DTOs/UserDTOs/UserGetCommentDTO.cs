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
    /// Made To see All comments made by specified user
    /// </summary>
    public class UserGetCommentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CommentListDTO> Comments { get; set; }
    }
}
