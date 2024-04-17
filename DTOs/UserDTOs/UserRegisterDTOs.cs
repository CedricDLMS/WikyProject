using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.UserDTOs
{
    /// <summary>
    /// Used to create new user register, crutials info need to initialize signInManager
    /// </summary>
    public class UserRegisterDTOs
    {
        public string Name {  get; set; }
        public DateTime BirthDate { get; set; }
        public string Mail {  get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }

    }
}
