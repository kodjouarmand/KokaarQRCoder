using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KokaarQrCoder.Utility.Helpers
{
    public static class CommonHelper
    {
        public static string TitleCase(string value)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(value).Replace("Of", "of").Replace("At", "at")
                .Replace("Door", "door").Replace("And", "and").Replace("Or", "or").Replace("To", "to")
                .Replace("For", "for").Replace("But", "but");
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Replace(" ", "_");
        }
    }
}
