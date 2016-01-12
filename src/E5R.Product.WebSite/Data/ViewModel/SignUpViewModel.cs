// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.ComponentModel.DataAnnotations;

namespace E5R.Product.WebSite.Data.ViewModel
{
    public class SignUpViewModel
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(60)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}