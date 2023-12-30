using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Placement
    {
        public int PlacementId { get; set; }
        public DateTime date { get; set; }
        public int Semester { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RegistrationLink { get; set; }
        public int CourseId { get; set; }
        public int FacultyId { get; set; }
    }
}
