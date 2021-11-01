using System.ComponentModel.DataAnnotations;
using IepProjekat.Controllers;
using Microsoft.AspNetCore.Mvc;
using IepProjekat.Models.Database;
using Microsoft.AspNetCore.Identity;


namespace IepProjekat.Models.View
{
    public class ProfileModel{
        
        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Display ( Name = "Password" ) ]
        [DataType(DataType.Password)]
        public string password { get; set; }
        
        [Display ( Name = "Confirm password" ) ]
        [Compare ( nameof ( password ), ErrorMessage = "Password and Confirm password fields must match!" ) ]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }

        [Display ( Name = "Old Password" ) ]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }
        
        [Required]
        [Display(Name = "Genre")]
        public string genre { get; set; }

        [HiddenInput]
        public string returnUrl { get; set; }
    }
}