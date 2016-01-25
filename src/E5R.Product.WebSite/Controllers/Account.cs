// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace E5R.Product.WebSite.Controllers
{
    using Core.Utils;
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
                
                // Generate temporary username
                var tempUserName = string.Concat(ProductOptions.AUTH_USER_TEMP_PREFIX, Guid.NewGuid().ToString("N"));
                var tempUserNameBase64 = tempUserName.ToBase64();
                
                Logger.LogVerbose($"TempUserName: { tempUserName }");
                Logger.LogVerbose($"TempUserName Base64: { tempUserNameBase64 }");
                
                var user = new User
                {
                    UserName = tempUserName,
                    Email = model.Email
                };

                user.Profile = new UserProfile
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                
                if (await UserManager.FindByNameAsync(user.UserName) != null)
                {
                    throw new Exception("Houston, i have a problem! Could not deduct a temporary username. Try again.");
                }
                
                var result = await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    Logger.LogInformation("User created a new account with password.");
                    
                    var confirmCode = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(Account.ConfirmEmail), nameof(Account), new { un = tempUserNameBase64, ct = confirmCode }, HttpContext.Request.Scheme);
                    
                    try
                    {
                        await EmailSender.SendEmailAsync(model.Email, "E5R confirm account",
                            $"Visit { callbackUrl } to confirm you account on E5R Development Team.");
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Houston, i have a problem!", e);
                    }
                        
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
                
                var user = AuthContext.Users.SingleOrDefault(w => w.UserName == model.UN.FromBase64());
                
                // HACK: Bugfix - Profile not automatic loaded
                AuthContext.UserProfiles.SingleOrDefault(p => p.UserId == user.Id);
                
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, $"User not found!");
                    return View(model);
                }
                                
                var result = await UserManager.ConfirmEmailAsync(user, model.CT);
                
                if (result.Succeeded)
                {
                    model.Confirmed = true;
                    model.FirstName = user.Profile.FirstName;
                }
                
                Logger.LogVerbose("Confirmation errors:");

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
        public IActionResult WaitConfirmation()
        {
            return View();
        }
    }
}