// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.ComponentModel.DataAnnotations;

namespace E5R.Product.WebSite.Data.ViewModel
{
    public class SignInModel
    {
        [Required]
        [Display(Name = "User name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}