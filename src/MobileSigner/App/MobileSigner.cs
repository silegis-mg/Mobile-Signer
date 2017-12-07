using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PCLStorage;
using Xamarin.Forms;
using Almg.MobileSigner.Pages;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Controllers;

namespace Almg.MobileSigner
{

    public class App : Application
	{
        private LoginController loginController = new LoginController();
        
        public App ()
		{
            bool loggedIn = loginController.IsLoggedIn();
            if (loggedIn)
			{
                ShowDocumentListPage();
			}
			else 
			{
				MainPage = new LoginPage();
			}
        }

		private void ShowDocumentListPage()
		{
			MainPage = new MainPage();
        }

        protected override void OnStart ()
		{
            ClearOldCacheFiles();
        }

		protected override void OnSleep ()
		{

		}

		protected override void OnResume ()
		{
            ClearOldCacheFiles();
        }

        private void ClearOldCacheFiles()
        {
            FileController fileController = new FileController();
            fileController.ClearOldCacheFiles();
        }
	}
}

