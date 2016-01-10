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
        private readonly AuthContext _authContext;
        
        public Profile(AuthContext authContext)
        {
            _authContext = authContext;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var user = _authContext.Users.SingleOrDefault(where => where.Id == User.GetUserId());

            if (user == null)
            {
                throw new Exception($"User not found for Id = { User.GetUserId() }");
            }

            return View(user);
        }
    }
}