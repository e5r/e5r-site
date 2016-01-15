// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.Logging;
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
            ILoggerFactory loggerFactory,
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
            Logger = loggerFactory.CreateLogger(nameof(Account));
        }
        
        private ILogger Logger { get; }
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
                    
                    if (!await UserManager.IsEmailConfirmedAsync(user))
                    {
                        await SignInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                        return View(model);
                    }
                    
                    Logger.LogInformation($"User [{ model.UserName }] logged in");
                    
                    if (Url.IsLocalUrl(urlReturn))
                    {
                        return Redirect(urlReturn);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Home.Index), nameof(Home));
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
            
            Logger.LogInformation("User logged out");
            
            return RedirectToAction(nameof(Home.Index), nameof(Home));
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
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Invalid password confirmation.");
                    return View(model);
                }
                
                if (AuthContext.Users.Count(c => string.Compare(c.Email, model.Email) == 0) > 0)
                {
                    ModelState.AddModelError(nameof(model.Email), "E-mail already registered");
                    return View(model);
                }
                
                var e5rNick = await UserBusiness.GenerateNickAsync(model.FirstName, (string nick) =>
                {
                    var count = AuthContext.Users.Count(c => c.UserName.Substring(1, 3) == nick);
                    
                    Logger.LogVerbose($"Callback calc Nick name count for [{ nick }] result [{ count }]");
                    
                    return count;
                });

                Logger.LogVerbose($"Nick name generated is [{ e5rNick }]");

                var user = new User
                {
                    UserName = e5rNick,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                
                var result = await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    Logger.LogInformation("User created a new account with password.");
                    
                    var confirmCode = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(Account.ConfirmEmail), nameof(Account), new { NickName = e5rNick, ConfirmationToken = confirmCode }, HttpContext.Request.Scheme);
                    
                    await EmailSender.SendEmailAsync(model.Email, "E5R confirm account",
                        $"Visit { callbackUrl } to confirm you account on E5R Development Team.");
                        
                    TempData["NickName"] = e5rNick;

                    return RedirectToAction(nameof(Account.WaitConfirmation), nameof(Account));
                }
                
                Logger.LogVerbose("Create user return a errors");

                foreach (var e in result.Errors)
                {
                    Logger.LogVerbose($"  -> { e.Description }");
                    ModelState.AddModelError(string.Empty, e.Description);
                }
            }
            
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                await SignInManager.SignOutAsync();
                
                var user = AuthContext.Users.SingleOrDefault(w => w.UserName == model.NickName);
                
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, $"User [{ model.NickName }] not found!");
                    return View(model);
                }
                
                var result = await UserManager.ConfirmEmailAsync(user, model.ConfirmationToken);
                
                if (result.Succeeded)
                {
                    model.Confirmed = true;
                    model.FirstName = user.FirstName;
                }
                
                Logger.LogVerbose("Confirmation errors.");

                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                }
            }

            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult WaitConfirmation()
        {
            ViewBag.NickName = TempData["NickName"];
            
            return View();
        }
    }
}