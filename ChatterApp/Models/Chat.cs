using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatterApp.Models
{
    public class Chat
    {
        [Key]
        public int ID { get; set; }
        [StringLength(150)]
        public string Message { get; set; }
        public DateTime DatePosted { get; set; }

        //[ForeignKey("ApplicationUser")]
        //public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}