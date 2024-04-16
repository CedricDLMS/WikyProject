using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Comment
    {
        public int Id { get; set; }

        // Comment Content is maximum 100 length
        [MaxLength(100)]
        public string Content { get; set; }

        
        // Foreign Key For Author 

        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        // Foreign Key for Article

        [ForeignKey("ArticleID")]
        public int ArticleID {  get; set; }
        public virtual Article Article { get; set; }

        public Comment() { }

    }
}
