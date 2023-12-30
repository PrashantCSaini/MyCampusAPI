using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Marks
    {
        public int AttachmentId { get; set; }
        public int StudentId { get; set; }
        public int AssignmentId { get; set; }
        public int MarksScored { get; set; }
    }
}
