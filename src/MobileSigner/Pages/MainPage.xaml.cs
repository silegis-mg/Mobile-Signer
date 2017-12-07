using Almg.MobileSigner.Controllers;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Almg.MobileSigner.Pages
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            var menu = new Menu();
            Master = menu;

            menu.MenuItemClicked += OnMenuClicked;
            GoToPendingSignatureList();
        }

        private void GoToPendingSignatureList()
        {
            var documents = new PendingRequestsPage(AppResources.SIGNATURE_REQUESTS);
            Detail = new NavigationPage(documents);
        }

        private void GoToFinishedSignatureRequests()
        {
            var documents = new SignedDocumentsPage();
            Detail = new NavigationPage(documents);
        }

        private void GoToConfig()
        {
			Detail = new NavigationPage(new ConfigPage());
        }

        private void OnMenuClicked(Menu.MenuItem item)
        {
            switch (item)
            {
                case Menu.MenuItem.LOGOUT:
                    Logout();
                    break;
                case Menu.MenuItem.REQUESTS:
                    GoToPendingSignatureList();
                    break;
                case Menu.MenuItem.SIGNED:
                    GoToFinishedSignatureRequests();
                    break;
                case Menu.MenuItem.CONFIG:
                    GoToConfig();
                    break;
            }
            IsPresented = false;
        }

        private void Logout()
        {
            Task<bool> task = DialogHelper.ShowConfirm(AppResources.APP_TITLE, AppResources.LOGOUT_CONFIRM);
            task.ContinueWith(t =>
            {
                if (t.IsCompleted && t.Result)
                {
                    Application.Current.Properties[Const.CONFIG_API_KEY] = null;
                    Application.Current.SavePropertiesAsync().Wait();
                    Pkcs12FileHelper.DeleteFile();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new LoginPage();
                    });
                }
            });
        }

    }
}