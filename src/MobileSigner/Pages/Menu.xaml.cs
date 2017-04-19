using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Almg.MobileSigner.Pages
{
    public partial class Menu : ContentPage
    {
        public enum MenuItem
        {
            REQUESTS, SIGNED, LOGOUT
        }

        public delegate void SelectedMenuItem(MenuItem menu);

        public event SelectedMenuItem MenuItemClicked;

        public Menu()
        {
            InitializeComponent();
            IUpdateApp updateApp = DependencyService.Get<IUpdateApp>();
            string currentVersion = updateApp.GetVersion();
            lblVersion.Text = AppResources.VERSION + ": " + currentVersion;
        }
        
        protected void OnViewCellTappedRequests(object sender, EventArgs e)
        {
            MenuItemClicked(MenuItem.REQUESTS);
        }

        protected void OnViewCellTappedSigned(object sender, EventArgs e)
        {
            MenuItemClicked(MenuItem.SIGNED);
        }

        protected void OnViewCellTappedLogout(object sender, EventArgs e)
        {
            MenuItemClicked(MenuItem.LOGOUT);
        }
    }
}
