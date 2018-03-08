using Acr.UserDialogs;
using Almg.MobileSigner.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using Acr.DeviceInfo;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Controllers;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Almg.MobileSigner.Pages
{
	public partial class LoginPage : ContentPage
	{
        private bool userNamePasswordLogin = true;

        public LoginPage()
		{
			InitializeComponent();

            entryUsername.Placeholder = AppResources.USER;
            entryPassword.Placeholder = AppResources.PASSWORD;
            entryToken.Placeholder = AppResources.TOKEN;

            btnLoginToken.Text = AppResources.LOGIN;
            btnLogin.Text = AppResources.LOGIN;

            this.Appearing += (object sender, System.EventArgs e) => entryUsername.Focus();
        }

		async public void OnLogin(object sender, EventArgs e)
		{
			string username = entryUsername.Text;
			string password = entryPassword.Text;
            string token = entryToken.Text;
            string model = DeviceInfo.Hardware.Manufacturer + " " + DeviceInfo.Hardware.Model;
            try
			{

                if (DeviceInfo.Connectivity.InternetReachability == NetworkReachability.NotReachable)
                {
                    UserDialogs.Instance.Alert(AppResources.INTERNET_UNAVAILABLE, AppResources.LOGIN_TITLE, AppResources.OK);
                    return;
                }

                using (DialogHelper.ShowProgress(AppResources.LOGGING_IN))
				{
                    string authToken;

                    if(userNamePasswordLogin)
                    {
                        authToken = await GetApiToken(username, password,
                                                      DeviceInfo.Hardware.DeviceId,
                                                      model);
                    } else
                    {
                        authToken = await GetApiToken(token.ToUpper(), 
                                                      DeviceInfo.Hardware.DeviceId,
                                                      model);
                    }

					Application.Current.Properties[Const.CONFIG_API_KEY] = authToken;
                    await Application.Current.SavePropertiesAsync();
                    Application.Current.MainPage = new CertPage();
				}
			}
            catch(HttpRequest.HttpException ex)
            {
                if(ex.HttpStatusCode==HttpStatusCode.Forbidden)
                {
                    UserDialogs.Instance.Alert(userNamePasswordLogin?AppResources.WRONG_USERNAME_PASSWORD:AppResources.TOKEN_INVALID_OR_EXPIRED, 
                                                AppResources.LOGIN_TITLE, AppResources.OK);
                } else
                {
                    UserDialogs.Instance.Alert(ExceptionHelper.GetMessage(AppResources.LOGIN_ERROR, ex), 
                                                AppResources.LOGIN_TITLE, AppResources.OK);
                }
            }
			catch (Exception ex)
			{
				UserDialogs.Instance.Alert(ExceptionHelper.GetMessage(AppResources.LOGIN_ERROR, ex), AppResources.LOGIN_TITLE, AppResources.OK);
			}
        }

        protected void OnChangeLoginMethod(object sender, EventArgs e)
        {
            userNamePasswordLogin = !userNamePasswordLogin;

            loginUserName.IsVisible = userNamePasswordLogin;
            loginToken.IsVisible = !userNamePasswordLogin;
        }

        bool firstTime = false;
		protected override void OnSizeAllocated(double width, double height)
		{
			if (!firstTime)
			{
                firstTime = true;
				imageLogo.HeightRequest = height / 3;
			}
			base.OnSizeAllocated(width, height);
		}

        private async Task<string> GetApiToken(string username, string password, string deviceId, string deviceDescription)
		{
            var postParams = new Dictionary<string, string> { { "username", username },
                                                              { "password", password },
                                                              { "deviceId", deviceId },
                                                              { "deviceDescription", deviceDescription }};

            var response = await HttpRequest.Post(EndPointHelper.GetLoginUserPassURL(), postParams);
            string token = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode || String.IsNullOrEmpty(token))
            {
                throw new Exception("Login Failed");
            } else
            {
                return token;
            }
		}

        private async Task<string> GetApiToken(string key, string deviceId, string deviceDescription)
        {
            var postParams = new Dictionary<string, string> { { "token", key },
                                                              { "deviceId", deviceId },
                                                              { "deviceDescription", deviceDescription }};

            var response = await HttpRequest.Post(EndPointHelper.GetLoginTokenURL(), postParams);
            string token = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode || String.IsNullOrEmpty(token))
            {
                throw new Exception("Login Failed");
            }
            else
            {
                return token;
            }
        }
    }
}
