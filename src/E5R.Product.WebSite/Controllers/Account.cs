// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;
using System.Linq;

namespace E5R.Product.WebSite.Controllers
{
    using Abstractions.Services;
    using Abstractions.Business;
    using Data.ViewModel;
    using Data.Model;
    using Data.Context;

    [Authorize]
    public class Account : Controller
    {
        public Account(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AuthContext authContext,
            IEmailSender emailSender,
            IUserBusiness userBusiness)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            AuthContext = authContext;
            EmailSender = emailSender;
            UserBusiness = userBusiness;
        }
        
        private UserManager<User> UserManager { get; set; }
        private SignInManager<User> SignInManager { get; set; }
        public AuthContext AuthContext { get; set; }
        public IEmailSender EmailSender { get; set; }
        public IUserBusiness UserBusiness { get; set; }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn(string urlReturn = null)
        {
            ViewBag.UrlReturn = urlReturn;
            
            return View(new SignInViewModel { RememberMe = true });
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string urlReturn = null)
        {
            await SignInManager.SignOutAsync();
            
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
                        return RedirectToAction(nameof(Home.Index), "Home");
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
            
            return RedirectToAction(nameof(Home.Index), "Home");
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                await SignInManager.SignOutAsync();

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Invalid password confirmation.");
                    return View(model);
                }
                
                var e5rNick = await UserBusiness.GenerateNickAsync(model.FirstName, (string nick) =>
                {
                    return AuthContext.Users.Count(c => c.UserName.Substring(0, 3) == nick);
                });

                var user = new User
                {
                    Accepted = false,
                    UserName = e5rNick,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var confirmCode = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(Account.ConfirmEmail), "Account", new { e5rNick = e5rNick, code = confirmCode }, HttpContext.Request.Scheme);

                    await EmailSender.SendEmailAsync(model.Email, "E5R confirm account",
                        $"Visit { callbackUrl } to confirm you account on E5R Development Team.");
                        
                    ViewData["e5rNick"] = e5rNick;

                    return RedirectToAction(nameof(Account.WaitConfirmation), "Controller");
                }

                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                }
            }

            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task ConfirmEmail(string e5rNick, string code)
        {
            await SignInManager.SignOutAsync();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public string WaitConfirmation()
        {
            var e5rNick = ViewData["e5rNick"];
            
            return $"See you e-mail to confirm account. You nick name is \"{ e5rNick }\"";
        }
    }
}