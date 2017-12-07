using Acr.DeviceInfo;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Pages;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Controllers
{
    class CertUpdateController
    {
        public async Task CheckCertUpdate()
        {
            try
            {
                if (DeviceInfo.Connectivity.InternetReachability == NetworkReachability.NotReachable)
                {
                    return;
                }

                string certHash = await Pkcs12FileHelper.GetSha1Hash();
                if (certHash != null)
                {

                    string remoteHash = await GetRemoteCertificateHash();
                    if(remoteHash==null)
                    {
                        DialogHelper.ShowAlertOK(AppResources.APP_TITLE, AppResources.CERT_UPDATE_FAILED);
                        return;
                    }

                    if(!certHash.Equals(remoteHash, StringComparison.OrdinalIgnoreCase))
                    {
                        bool yes = await DialogHelper.ShowConfirm(AppResources.APP_TITLE, AppResources.CERT_UPDATE_AVAILABLE);
                        if (yes)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Application.Current.MainPage = new CertPage();
                            });
                        }
                    } else
                    {
                        DialogHelper.ShowAlertOK(AppResources.APP_TITLE, AppResources.CERT_UPDATE_NOT_REQUIRED);
                    }
                }
            } catch(Exception)
            {
                DialogHelper.ShowAlertOK(AppResources.APP_TITLE, AppResources.CERT_UPDATE_FAILED);
            }
        }

        private async Task<string> GetRemoteCertificateHash()
        {
            return await HttpRequest.GetStr(EndPointHelper.GetCertificateHashURL());
        }
    }
}
