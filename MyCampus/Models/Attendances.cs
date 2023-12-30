using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Attendances
    {
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public int CourseId { get; set; }
        public int Semester { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public char Attendance { get; set; }
    }
}
