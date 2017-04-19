using Acr.DeviceInfo;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Controllers
{
    public class AppUpdateController
    {
        public async Task CheckUpdate()
        {
            if(DeviceInfo.Connectivity.InternetReachability == NetworkReachability.NotReachable)
            {
                return;
            }

			IUpdateApp updateApp = DependencyService.Get<IUpdateApp>();
			string currentVersion = updateApp.GetVersion();
            string version = await HttpRequest.GetStr(EndPointHelper.GetCheckAppVersionUrl(Device.OS));

            if (parseVersion(currentVersion) < parseVersion(version))
            {
                bool yes = await DialogHelper.ShowConfirm(AppResources.APP_TITLE, AppResources.UPDATE_APP);
                if (yes)
                {
                    updateApp.Update();
                }
            }
        }

        private int parseVersion(string version)
        {
            return int.Parse(version.Replace(".", ""));
        }
    }
}
