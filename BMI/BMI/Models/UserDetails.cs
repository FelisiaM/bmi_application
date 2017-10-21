using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMI.Models
{
    public class UserDetails
    {
        [Required()]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required()]
        [Display(Name = "Height", Description = "Put your height in metres")]
        public double Height { get; set; }

        [Required()]
        [Display(Name = "Weight", Description = "Put your weight in kilograms")]
        public double Weight { get; set; }
    }
}
