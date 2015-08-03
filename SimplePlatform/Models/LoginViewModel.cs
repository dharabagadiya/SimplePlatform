using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Security.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "A valid email address is required.")]
        [Display(Name = "email")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}