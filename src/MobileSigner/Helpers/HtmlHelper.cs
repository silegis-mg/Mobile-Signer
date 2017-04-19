using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Helpers
{
    public class HtmlHelper
    {
        public static string StripTags(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string Decode(string input)
        {
            return System.Net.WebUtility.HtmlDecode(input);
        }

        public static string ToPlainText(string html)
        {
            if (!string.IsNullOrEmpty(html)) {
                return Decode(StripTags(html));
            } else
                return html;
        }
    }
}
