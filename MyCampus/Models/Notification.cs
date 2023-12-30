using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public int Semester { get; set; }
        public int FacultyId { get; set; }

    }
}
