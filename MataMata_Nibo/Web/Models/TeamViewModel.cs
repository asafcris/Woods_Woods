using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class TeamViewModel
    {
        public  long Id { get; set; }
        [RegularExpression("^[A-Za-zÀ-Üà-ü0-9\\s]{6,50}$",
            ErrorMessage = "Error. Enter the Name Correctly")]

        [Required(ErrorMessage = "Please enter the name of the Team")]
        [Display(Name = "Team Name:")]
        public  string Name { get; set; }
    }
}