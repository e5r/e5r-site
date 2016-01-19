// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace E5R.Product.WebSite.Core.Business
{
    using Core.Utils;
    using Abstractions.Business;

    public class UserBusiness : IUserBusiness
    {
        public UserBusiness(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(nameof(UserBusiness));
        }

        public ILogger Logger { get; }
        
        public Task<string> GenerateNickAsync(string firstName, Func<string, int> nickCountCallback)
        {
            Logger.LogDebug($"Generating nick for [{ firstName }]");
            
            return Task<string>.Factory.StartNew(() =>
            {
                if ((firstName ?? string.Empty).Length < 3)
                {
                    throw new ArgumentException("Invalid param value", nameof(firstName));
                }

                var firstChar = firstName.First();
                var lastChar = firstName.Last();
                var middleCount = firstName.Length - 2;
                var nickName = $"{ firstChar }{ middleCount }{ lastChar }"
                    .NormalizeName()
                    .ToLower();

                var incrementalCount = nickCountCallback(nickName);
                
                if (incrementalCount > 0)
                {
                    nickName += incrementalCount.ToString();
                }
                
                Logger.LogDebug($"  -> Generated nick [{ nickName }]");
                
                return nickName;
            });
        }
    }
}
