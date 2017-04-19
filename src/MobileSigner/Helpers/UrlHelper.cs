using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Helpers
{
    public class UrlHelper
    {
        public static string UrlEncode(string input)
        {
            return System.Net.WebUtility.UrlEncode(input);
        }
    }
}
