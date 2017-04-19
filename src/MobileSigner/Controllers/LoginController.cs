using Almg.MobileSigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Controllers
{
    public class LoginController
    {
        public bool IsLoggedIn()
        {
            return App.Current.Properties.ContainsKey(Const.CONFIG_OK) &&
                              (bool)App.Current.Properties[Const.CONFIG_OK];
        }
    }
}
