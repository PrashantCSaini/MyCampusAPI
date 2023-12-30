using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class StudentMaster
    {
        public int StudentId { get; set; }
        public int AdmissionYear { get; set; }
        public string Branch { get; set; }
        public int CurrentSemester { get; set; }
        public char Division { get; set; }
        public int RollNo { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string ResidentialAddress { get; set; }
        public string NativeAddress { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string MotherTongue { get; set; }
        public string Discipline { get; set; }
        public DateTime JoiningDate { get; set; }
        public string PhysicallyHandicapped { get; set; }
        public string MentorName { get; set; }
        public int LoginId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
