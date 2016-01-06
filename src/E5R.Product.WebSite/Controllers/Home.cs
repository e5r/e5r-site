// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Mvc;

namespace E5R.Product.WebSite.Controllers
{
    public class Home : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}