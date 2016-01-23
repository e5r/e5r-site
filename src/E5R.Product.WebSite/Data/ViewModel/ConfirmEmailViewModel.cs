// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.ComponentModel.DataAnnotations;

namespace E5R.Product.WebSite.Data.ViewModel
{
    public class ConfirmEmailViewModel
    {
        [Required]
        public string TempName { get; set; }
        
        [Required]
        public string ConfirmationToken { get; set; }
        
        public bool Confirmed { get; set; }
        public string FirstName { get; set; }
    }
}