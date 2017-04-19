using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Components
{
    public class CustomWebView: WebView
    {
        public bool OverviewMode { get; set; }

        public CustomWebView()
        {
            OverviewMode = false;
        }
    }
}
