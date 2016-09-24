using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class Tournament
    {
        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Sport { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? RegistrationDeadline { get; set; }
        public string Address { get; set; }
        //[ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
    }
}