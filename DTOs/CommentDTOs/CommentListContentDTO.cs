using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CommentDTOs
{
    /// <summary>
    /// Comment DTO used for listing , keeping ID cause easy to get
    /// </summary>
    public class CommentListContentDTO
    {
        public int ID { get; set; }
        public string authorName {  get; set; }
        public string content { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
