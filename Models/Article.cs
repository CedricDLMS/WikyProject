using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Article
    {

        public int Id { get; set; }

        // DATE

        private DateTime creationDate;
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                EditDate = value;
            }
        }

        private DateTime editDate;
        public DateTime EditDate
        {
            get { return editDate; }
            set { editDate = value; }
        }

        // CONTENT
        [StringLength(40)]
        public string Title { get; set; }
        public string Content { get; set; }

        // Return a list of comments
        public List<Comment>? Comments { get; set; }

        // Foreign Keys for priority and Theme
        [ForeignKey("Theme ID")]
        public int? ThemeID { get; set; }
        public virtual Theme Theme { get; set; }

        // PRIORITY

        public required Priority Priority
        {
            get { return Priority; }
            set { Priority = value; }
        }
        
        // Foreign Key For Author 
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public Article() { }

    }
}
