using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public DateTime UploadDate { get; set; }
        public string AttachmentUrl { get; set; }
        public int MarksScored { get; set; }
        public int StudentId { get; set; }
        public int AssignmentId { get; set; }

    }
}
