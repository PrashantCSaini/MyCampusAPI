using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class FacultyMaster
    {
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
        public string Education { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int CourseId { get; set; }
        public int LoginId { get; set; }
    }
}
