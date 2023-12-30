using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int TotalMarks { get; set; }
        public int CourseId { get; set; }
        public int Semester { get; set; }
        public int FacultyId { get; set; }
        public int SubjectId { get; set; }

    }
}
