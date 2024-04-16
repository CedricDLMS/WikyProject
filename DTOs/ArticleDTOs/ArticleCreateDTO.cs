using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Here we only use data that would be user for creation by user
    /// Title, content, ThemeID, priority
    /// </summary>
    public class ArticleCreateDTO
    {

        public string Title { get; set; }
        public string Content { get; set; }
        public int? ThemeID { get; set; } // Seulement l'ID pour éviter la surcharge de données
        public Priority Priority { get; set; }
    }
}
