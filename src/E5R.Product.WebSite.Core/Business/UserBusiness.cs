// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Linq;
using System.Threading.Tasks;

namespace E5R.Product.WebSite.Core.Business
{
    using Abstractions.Business;

    public class UserBusiness : IUserBusiness
    {
        public Task<string> GenerateNickAsync(string firstName, Func<string, int> nickCountCallback)
        {
            return new Task<string>(() =>
            {
                if ((firstName ?? string.Empty).Length < 3)
                {
                    throw new Exception($"Invalid param { nameof(firstName) }");
                }

                var firstChar = firstName.First();
                var lastChar = firstName.Last();
                var middleCount = firstName.Length - 2;
                var nickName = $"{ firstChar }{ middleCount }{ lastChar }".ToLower();

                var incrementalCount = nickCountCallback(nickName);

                if (incrementalCount > 0)
                {
                    nickName += incrementalCount.ToString();
                }

                return nickName;
            });
        }
    }
}
