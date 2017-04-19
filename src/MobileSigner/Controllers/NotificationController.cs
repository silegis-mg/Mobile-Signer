using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Controllers
{
    public class NotificationController
    {
        private LoginController loginCrtl = new LoginController();

        public async Task<bool> SendNewTokenToServer(string token, string version)
        {
            if(!string.IsNullOrEmpty(token))
            {
                App.Current.Properties[Const.CONFIG_GCM_TOKEN] = token;
                await App.Current.SavePropertiesAsync();
                await SendCurrentToken(token, version);
            }
            return false;
        }

        public async Task<bool> SendCurrentToken(string token, string version)
        {
            if (loginCrtl.IsLoggedIn())
            {
                try
                {
                    var resp = await HttpRequest.Post(EndPointHelper.GetNotificationTokenSubscribe(), new Dictionary<string, string>()
                    {
                        { "token", token },
                        { "version", version }
                    });
                    return resp.IsSuccessStatusCode;
                } catch(Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
