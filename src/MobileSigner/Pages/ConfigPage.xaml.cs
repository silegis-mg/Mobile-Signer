using Almg.MobileSigner.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Controllers;
using Almg.MobileSigner.Services;

namespace Almg.MobileSigner.Pages
{
	public partial class ConfigPage : ContentPage
	{
		public ConfigPage()
		{
			InitializeComponent();
            lblMessage.Text = AppResources.APP_IS_BEING_CONFIGURED;
            LoadConfiguration();
        }

        void LoadConfiguration()
        {
            DownloadCertificate()
                .ContinueWith(certificateTask => {
                    if (!certificateTask.IsFaulted && certificateTask.Result != null)
                    {
                        DialogHelper.AskForPassword(AppResources.INPUT_PKCS12_PASSWORD, AppResources.CONFIGURATION_TITLE).ContinueWith(
                            senhaTask => {
                                if (!senhaTask.IsFaulted)
                                {
                                    try
                                    {
                                        Stream certificate = certificateTask.Result;
                                        if (VerifyCertificate(certificate, senhaTask.Result))
                                        {
                                            Pkcs12FileHelper.Save(certificate).ContinueWith(saveTask => {
                                                if(!saveTask.IsFaulted)
                                                {
                                                    Application.Current.Properties[Const.CONFIG_OK] = true;
                                                    Application.Current.Properties[Const.CONFIG_VERSION] = Const.CONFIG_VALUE_CURRENT_VERSION;
                                                    Application.Current.SavePropertiesAsync().Wait();
                                                    //Sends the current gcm token to the server if it exists
                                                    SendNotificationToken().Wait();
                                                    OpenDocumentListPage();
                                                } else
                                                {
                                                    ShowErrorMessage(ExceptionHelper.GetMessage(AppResources.CERTIFICATE_SAVE_ERROR, saveTask.Exception.InnerException), 
                                                                     () => Application.Current.MainPage = new LoginPage());
                                                }
                                            });
                                        }
                                    }
                                    catch(IOException)
                                    {
                                        //probably a wrong pkcs12 password was supplied. Try again.
                                        ShowErrorMessage(AppResources.WRONG_PKCS12_PASSWORD, () => LoadConfiguration());
                                    }
                                    catch (Exception e)
                                    {
                                        string errorMsg = ExceptionHelper.GetMessage(AppResources.WRONG_PKCS12_PASSWORD, e);
                                        ShowErrorMessage(errorMsg, () => Application.Current.MainPage = new LoginPage());                                              
                                    }
                                } else
                                {
                                    string errorMsg = ExceptionHelper.GetMessage(AppResources.ASK_PASSWORD_ERROR, senhaTask.Exception.InnerException);
                                    ShowErrorMessage(errorMsg, () => Application.Current.MainPage = new LoginPage());
                                }
                            });
                    }
                    else
                    {
                        string errorMsg = ExceptionHelper.GetMessage(AppResources.CERTIFICATE_LOAD_ERROR, certificateTask.Exception.InnerException);
                        ShowErrorMessage(errorMsg, () => Application.Current.MainPage = new LoginPage());
                    }
                });
        }

        async Task<bool> SendNotificationToken()
        {
            IUpdateApp updateApp = DependencyService.Get<IUpdateApp>();
            if (App.Current.Properties.ContainsKey(Const.CONFIG_GCM_TOKEN))
            {
                string token = (string)App.Current.Properties[Const.CONFIG_GCM_TOKEN];
                if(!string.IsNullOrEmpty(token))
                {
                    return await new NotificationController().SendCurrentToken(token, updateApp.GetVersion());
                }
            }
            return false;
        }

        void OpenDocumentListPage()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new MainPage();
            });
        }

        bool VerifyCertificate(Stream stream, string senha)
        {
            var pfxEntry = Pkcs12FileHelper.Load(stream, senha);
            return pfxEntry != null;
        }

        async Task<Stream> DownloadCertificate()
        {
            HttpResponseMessage response = await HttpRequest.GetHttpResponse(EndPointHelper.GetCertificateURL());
			Stream stream = await response.Content.ReadAsStreamAsync();
			return stream;
        }

		void ShowErrorMessage(string message, Action onClose)
		{
			AlertConfig config = new AlertConfig()
									   .SetMessage(message)
									   .SetOkText(AppResources.OK)
									   .SetTitle(AppResources.CONFIGURATION_TITLE)
									   .SetAction(onClose);

			UserDialogs.Instance.Alert(config);
		}
	}
}
