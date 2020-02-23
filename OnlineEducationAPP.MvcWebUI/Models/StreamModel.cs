using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Models
{
    public class StreamModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string Endpoint { get; set; }
        public string StreamKey { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartTime { get; set; }
    }
}
