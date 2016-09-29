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

        [Required(ErrorMessage=" Name of the tournament is required")]
        public string Name { get; set; }

        [Required(ErrorMessage="Sports discipline is required")]
        public string Sport { get; set; }

        [DataType(DataType.Date)]
        [ValidateFutureDate]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage="Date is required")]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateLessThan("Date", AllowEquality = true)]
        [ValidateFutureDate]
        [Display(Name = "Registration deadline")]
        [Required(ErrorMessage="Registration deadline is required")]
        public DateTime RegistrationDeadline { get; set; }

        public string Address { get; set; }

        [Range(2, 8000000000, ErrorMessage="Participants count must be between 2 and 8000000000")]
        [Display(Name = "Max. participants")]
        public int? MaxPlayersNumber { get; set; }
        //[ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
    }
}