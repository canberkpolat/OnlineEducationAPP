using Microsoft.AspNetCore.Identity;
using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Entity
{
    public class Stream
    {
        public int Id { get; set; }
        public string StreamName { get; set; }
        public string LiveStreamEndpoint { get; set; }
        public string VideoOnDemandEndpoint { get; set; }
        public string StreamKey { get; set; }
        public bool IsActive { get; set; }
        public int AmaountShown { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User  { get; set; }
        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }


    }
}
