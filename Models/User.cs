using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        
        // List articles made by the user
        public List<Article>? Articles { get; set; }

        // List Comments made by user
        public List<Comment>? Comments { get; set; }


        // Foreign Key to AppUser
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public User() { }
    }
}
