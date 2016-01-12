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
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(object model)
        {
            // Send mail with Mailgun
            // * https://mailgun.com
            // * https://documentation.mailgun.com/quickstart-sending.html#send-via-api
            
            await SignInManager.SignOutAsync();
            
            return View(model);
        }
        
        // public static IRestResponse SendSimpleMessage()
        // {
        //     RestClient client = new RestClient();
        //     client.BaseUrl = new Uri("https://api.mailgun.net/v3");
        //     client.Authenticator =
        //             new HttpBasicAuthenticator("api",
        //                                        "YOUR_API_KEY");
        //     RestRequest request = new RestRequest();
        //     request.AddParameter("domain",
        //                          "YOUR_DOMAIN_NAME", ParameterType.UrlSegment);
        //     request.Resource = "{domain}/messages";
        //     request.AddParameter("from", "Excited User <mailgun@YOUR_DOMAIN_NAME>");
        //     request.AddParameter("to", "bar@example.com");
        //     request.AddParameter("to", "YOU@YOUR_DOMAIN_NAME");
        //     request.AddParameter("subject", "Hello");
        //     request.AddParameter("text", "Testing some Mailgun awesomness!");
        //     request.Method = Method.POST;
        //     return client.Execute(request);
        //     
        //     /*
        //     using System.Net.Http;
        //     using Microsoft.AspNet.Http; 
        //     
        //     HttpClient client = new HttpClient();
        //     */
        //     
        //     /*
        //     curl -s --user 'api:YOUR_API_KEY' \
        //         https://api.mailgun.net/v3/YOUR_DOMAIN_NAME/messages \
        //         -F from='Excited User <mailgun@YOUR_DOMAIN_NAME>' \
        //         -F to=YOU@YOUR_DOMAIN_NAME \
        //         -F to=bar@example.com \
        //         -F subject='Hello' \
        //         -F text='Testing some Mailgun awesomness!'
        //      */
        // }
    }
}