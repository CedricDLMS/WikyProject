using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CommentDTOs
{
    /// <summary>
    /// DTO to update a comment, only needed values
    /// </summary>
    public class CommentEditByUserDTO
    {
        public string Content { get; set; }
    }
}
