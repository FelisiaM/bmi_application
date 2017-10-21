using System.ComponentModel.DataAnnotations;

namespace BMI.Models
{
    public class UserDetails
    {
        [Required()]
        [Display(Name = "Height", Description = "Put your height in metres")]
        public double Height { get; set; }

        [Required()]
        [Display(Name = "Weight", Description = "Put your weight in kilograms")]
        public double Weight { get; set; }
    }
}
