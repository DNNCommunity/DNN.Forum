using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNetNuke.Forum.Library
{
    public class TextUtilityClass
    {
        public static string StripHTML(string sText)
        {
            if (string.IsNullOrEmpty(sText))
                return string.Empty;

            const string pattern = @"<(.|\n)*?>";
            return Regex.Replace(sText, pattern, string.Empty).Trim();
        }
    }
}
