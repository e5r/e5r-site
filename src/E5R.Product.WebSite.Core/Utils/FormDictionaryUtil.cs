// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Collections.Generic;

namespace E5R.Product.WebSite.Core.Utils
{
    public class FormDictionaryUtil : Dictionary<string, string>
    {
        public FormDictionaryUtil Set(string key, string value)
        {
            base.Add(key, value);
            return this;
        }
    }
}