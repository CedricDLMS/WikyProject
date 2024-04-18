using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Here we show only crutials data for a supposed list
    /// We assume we only want , Id , creationDate,EditDate,Title,Theme,Priority
    /// No need of content here , would be too much server consuming
    /// </summary>
    public class ArticleListDTO
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string AuthorName {  get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public Priority Priority { get; set; }

    }
}
