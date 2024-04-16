using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CommentDTOs
{
    /// <summary>
    /// Made To see details about specified Comment
    /// </summary>
    public class CommentGetDTO
    {
        public int ID { get; set; }
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; }
    }
}
