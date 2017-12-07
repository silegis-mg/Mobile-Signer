using Almg.MobileSigner.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Almg.MobileSigner.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage : ContentPage
    {
        private CertUpdateController certController = new CertUpdateController();

        public ConfigPage()
        {
            InitializeComponent();
        }

        private async void OnConfigClick(object sender, EventArgs e)
        {
            btnLoadCert.IsEnabled = false;
            await certController.CheckCertUpdate();
            btnLoadCert.IsEnabled = true;
        }
    }
}