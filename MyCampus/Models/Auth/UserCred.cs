using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models.Auth
{
    public class UserCred
    {
        public string username { get; set; }
        public string password { get; set; }
        public char type { get; set; }
    }
}
