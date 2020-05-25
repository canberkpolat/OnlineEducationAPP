using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public string Messages { get; set; }
        public DateTime? SendTime { get; set; }
        public DateTime? ReceiveTime { get; set; }
       
        
        public string ReceiverId { get; set; }
        public virtual ApplicationUser ReceiverUser{ get; set; }

        public string SenderId { get; set; }
        public virtual ApplicationUser SenderUser { get; set; }

    }
}
