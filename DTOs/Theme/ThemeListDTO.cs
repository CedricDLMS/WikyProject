using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Theme
{
    /// <summary>
    /// used to list all themes , with id , and theme name
    /// </summary>
    public class ThemeListDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
    }
}
