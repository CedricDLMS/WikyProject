using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ArticleDTOs
{
    /// <summary>
    /// Made To be Used in User ListDTO, output id , create time, title, theme
    /// </summary>
    public class ArticleListSimpleDTO
    {
        public int ID { get; set; }
        public DateTime CreationDate {  get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        
    }
}
