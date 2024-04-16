using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Data that can be updated by an admin 
    /// </summary>
    public class ArticleUpdateAdminDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? ThemeID { get; set; }
        public Priority Priority { get; set; }
        public DateTime EditDate { get; set; } // Potentially updated to reflect changes
    }
}
