// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace E5R.Product.WebSite.Controllers
{
    [Authorize]
    public class Profile : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}