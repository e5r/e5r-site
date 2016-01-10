// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;

namespace E5R.Product.WebSite.Controllers
{
    using Data.ViewModel;
    using Data.Model;

    [Authorize]
    public class Account : Controller
    {
        public Account(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        
        public UserManager<User> UserManager { get; set; }
        public SignInManager<User> SignInManager { get; set; }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string urlReturn = null)
        {
            ViewBag.UrlReturn = urlReturn;
            
            await SignInManager.SignOutAsync();
            
            return View(new SignInViewModel { RememberMe = true });
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string urlReturn = null)
        {
            ViewBag.UrlReturn = urlReturn;

            if (ModelState.IsValid)
            {
                var signed = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                
                if (signed.Succeeded)
                {
                    var user = await UserManager.FindByNameAsync(model.UserName);
                    
                    if (!user.Accepted)
                    {
                        await SignInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                        return View(model);
                    }
                    
                    if (Url.IsLocalUrl(urlReturn))
                    {
                        return Redirect(urlReturn);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await SignInManager.SignOutAsync();
            
            return RedirectToAction("index", "home");
        }
    }
}