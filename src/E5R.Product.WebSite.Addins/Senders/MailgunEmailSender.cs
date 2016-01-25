// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;

namespace E5R.Product.WebSite.Addins.Senders
{
    using Core.Utils;
    using Abstractions.Services;
    
    /// <summary>
    /// Send e-mail with Mailgun service.
    /// https://mailgun.com
    /// </summary>
    public class MailgunEmailSender : IEmailSender
    {
        public MailgunEmailSender(ILoggerFactory loggerFactory, IOptions<MailgunOptions> options)
        {
            Logger = loggerFactory.CreateLogger(nameof(MailgunEmailSender));
            Options = options.Value;
        }
        
        private ILogger Logger { get; }
        private MailgunOptions Options { get; }
        
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            Logger.LogVerbose($"Sending e-mail to { email }");
            
            var domain = Options.Domain;
            var uri = $"{ domain }/messages";
            var key = Options.AppKey;
            var auth = $"api:{ key }".ToBase64();
            var client = new HttpClient();
            
            client.BaseAddress = new Uri(Options.BaseAddress); 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

            var response = await client.PostAsync(uri, new FormUrlEncodedContent(new FormDictionaryUtil()
                .Set("from", Options.From)
                .Set("to", email)
                .Set("subject", subject)
                .Set("text", message)
            ));
            var requestedUri = response.RequestMessage.RequestUri.AbsoluteUri;
            var responseString = await response.Content.ReadAsStringAsync();

            Logger.LogVerbose($"Sending HTTP POST to [{ uri }]...");
            Logger.LogVerbose($"  -> Request.Uri [{ requestedUri }]");
            Logger.LogVerbose($"  -> Response.Statuscode [{ response.StatusCode }]");
            Logger.LogVerbose($"  -> Response.Content [{ responseString }]");
            
            response.EnsureSuccessStatusCode();
        }
    }
}
