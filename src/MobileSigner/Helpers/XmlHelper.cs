using Almg.MobileSigner.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Helpers
{
    public class XmlHelper
    {
        public static string Transform(string xslt, string xml)
        {
            var transformer = DependencyService.Get<IXslTransformer>();
            return transformer.Transform(xslt, xml);
        }
    }
}
