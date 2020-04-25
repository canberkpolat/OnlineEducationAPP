using Microsoft.AspNetCore.Identity;
using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Entity
{
    public class Stream
    {
        public int Id { get; set; }
        public string StreamName { get; set; }
        public string Endpoint { get; set; }
        public string StreamKey { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User  { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }


    }
}
