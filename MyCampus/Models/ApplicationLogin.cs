using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class ApplicationLogin
    {
        public int LoginId { get; set; }
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }
        public char UserType { get; set; }
    }
}
