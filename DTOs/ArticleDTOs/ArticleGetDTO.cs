using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Here we show all the informations of the Article for data purposes
    /// </summary>
    public class ArticleGetDTO
    {

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Theme { get; set; }
        public Priority Priority { get; set; }
    }
}
