using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CommentDTOs
{
    /// <summary>
    /// DTO used when you wanna see all comments made by an user, we suppose we only want update Time for them , more logical for ordering purposes
    /// </summary>
    public class CommentFindByUserDTO
    {
        public int Id { get; set; }
        public int ArticleID { get; set; }
        public string ArticleTitle {  get; set; }
        public DateTime LastUpdate { get; set; }
        public string Content { get; set; }

    }
}
