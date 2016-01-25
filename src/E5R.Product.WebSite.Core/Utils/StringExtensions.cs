// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Text;
using System.Globalization;

namespace E5R.Product.WebSite.Core.Utils
{
    public static class StringExtensions
    {
        public static string NormalizeName(this string self)
        {
            var builder = new StringBuilder();

            foreach (var c in self.Normalize(NormalizationForm.FormD))
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.EnclosingMark:
                        break;

                    default:
                        builder.Append(c);
                        break;
                }
            }

            return builder.ToString();
        }
        
        public static string ToBase64(this string self)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(self));
        }
        
        public static string FromBase64(this string self)
        {
            var bytes = Convert.FromBase64String(self);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}