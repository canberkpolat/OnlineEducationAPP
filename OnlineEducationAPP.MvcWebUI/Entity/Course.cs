using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Entity
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Stream> Streams { get; set; }
    }
}
