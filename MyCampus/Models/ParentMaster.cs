using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class ParentMaster
    {
        public int ParentId { get; set; }
        public string FatherName { get; set; }
        public string FatherEducation { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherOfficeAddress { get; set; }
        public string FatherMobileNo { get; set; }
        public string FatherEmail { get; set; }
        public string MotherName { get; set; }
        public string MotherEducation { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherOfficeAddress { get; set; }
        public string MotherMobileNo { get; set; }
        public string MotherEmail { get; set; }
        public int StudentId { get; set; }
        public int LoginId { get; set; }
    }
}
