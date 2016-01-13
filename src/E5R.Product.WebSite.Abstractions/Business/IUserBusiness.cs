// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Threading.Tasks;

namespace E5R.Product.WebSite.Abstractions.Business
{
    public interface IUserBusiness
    {
        Task<string> GenerateNickAsync(string firstName, Func<string, int> nickCountCallback);
    }
}
