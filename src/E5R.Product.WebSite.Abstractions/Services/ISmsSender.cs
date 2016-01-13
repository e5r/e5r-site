// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Threading.Tasks;

namespace E5R.Product.WebSite.Abstractions.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string phone, string message);
    }
}
