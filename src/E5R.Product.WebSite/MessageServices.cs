// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Threading.Tasks;

namespace E5R.Product.WebSite
{
    public class MessageServices
    {
        public async static Task SendEmailAsync(string email, string subject, string message)
        {
            
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
        //     // http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
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