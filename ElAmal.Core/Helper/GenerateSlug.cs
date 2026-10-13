using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElAmal.Core.Helper
{
    public static class GenerateSlug
    {
        public static string Generate(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // convert text letters  to lower case
            text = text.ToLowerInvariant().Trim();

            // Normalization => Remove accents
            var normalized = text.Normalize(NormalizationForm.FormD);
            
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            text = sb.ToString().Normalize(NormalizationForm.FormC);

            // Replace spaces with hyphens
            text = Regex.Replace(text, @"\s+", "-");

            // Keep English letters, Arabic characters, numbers, and hyphens
            text = Regex.Replace(text, @"[^a-z0-9\u0600-\u06FF-]", "");

            // Remove duplicate hyphens
            text = Regex.Replace(text, @"-+", "-");

            // Remove hyphens from the beginning and end
            return text.Trim('-');


        }
    }
}
