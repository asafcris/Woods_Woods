using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class KeyViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual long TeamOneId { get; set; }
        [Display(Name = "Team Name One:")]
        public virtual string TeamOne { get; set; }
        public virtual long TeamTwoId { get; set; }
        [Display(Name = "Team Name Two:")]
        public virtual string TeamTwo { get; set; }
        [DataType(DataType.Currency, ErrorMessage = "Only numbers")]
        [Range(0, 99)]
        public virtual int TeamGolsOne { get; set; }
        [DataType(DataType.Currency, ErrorMessage = "Only numbers")]
        [Range(0, 99)]
        public virtual int TeamGolsTwo { get; set; }
        public virtual int Keys { get; set; }
    }
}