using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CommentDTOs
{
    /// <summary>
    /// Comment List Without Content, easier , faster, love
    /// </summary>
    public class CommentListDTO
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public DateTime Created {  get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
