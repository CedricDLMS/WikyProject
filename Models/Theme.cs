using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // List articles with this theme
        public List<Article>? Articles { get; set; }
        public Theme() { }
    }
}
