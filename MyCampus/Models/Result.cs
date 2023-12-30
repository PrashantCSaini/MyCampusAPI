using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public string ExamType { get; set; }
        public string SubjectName { get; set; }
        public int MarksScored { get; set; }
        public int TotalMarks { get; set; }
        public int StudentId { get; set; }
    }
}
