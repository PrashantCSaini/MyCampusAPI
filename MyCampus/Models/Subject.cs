using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int InternalMarks { get; set; }
        public int ExternalTheoryMarks { get; set; }
        public int ExternalPracticalMarks { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }

    }
}
