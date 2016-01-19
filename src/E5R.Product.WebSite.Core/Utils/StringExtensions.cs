// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

#if DNXCORE50
using System;
#endif
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
    }
}