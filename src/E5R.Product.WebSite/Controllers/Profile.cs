// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Security.Claims;
using System.Linq;

namespace E5R.Product.WebSite.Controllers
{
    using System;
    using Data.Context;

    [Authorize]
    public class Profile : Controller
    {
        public Profile(AuthContext authContext)
        {
            AuthContext = authContext;
        }
        
        private AuthContext AuthContext { get; set; }
        
        [HttpGet]
        public IActionResult Index()
        {
            var user = AuthContext.Users.SingleOrDefault(where => where.Id == User.GetUserId());

            if (user == null)
            {
                throw new Exception($"User not found for Id = { User.GetUserId() }");
            }

            return View(user);
        }
    }
}