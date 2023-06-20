using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OTP.Models.DTO
{
    public class SignUp
    {
        //[Required]
        //public string FirstName { get; set; }
        //[Required]
        //public string LastName { get; set; }
        [Required, EmailAddress(ErrorMessage = "please enter a valid email")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "please enter strong password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "please ccomfirm your password")]
        [Compare("Password", ErrorMessage = "password does not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

}
