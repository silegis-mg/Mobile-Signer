using Almg.MobileSigner.Components;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Services;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Pages.Signature
{
    public class TextPage: ContentPage
    {
        private BaseInboxMessage request;

        public TextPage(BaseInboxMessage request, Document document, bool showSignButton, EventHandler OnAssinar)
        {
            this.request = request;
            this.Title = "Texto";
            if (Device.OS == TargetPlatform.iOS)
                this.Icon = (FileImageSource)FileImageSource.FromFile("text.png");

            var html = new HtmlWebViewSource();
            
            var browser = new CustomWebView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = html,
                MinimumHeightRequest = 300
            };

            if (showSignButton)
            {
                Button btnAssinar = new Button()
                {
                    Text = AppResources.SIGN_DOCUMENT,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                var relativeLayout = new RelativeLayout();

                relativeLayout.Children.Add(browser, Constraint.RelativeToParent((parent) => {
                    return parent.X;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Y;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Height - 50;
                }));

                relativeLayout.Children.Add(btnAssinar, Constraint.RelativeToView(browser, (Parent, sibling) => {
                    return 0;
                }), Constraint.RelativeToView(browser, (parent, sibling) => {
                    return sibling.Height;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }), Constraint.Constant(50));

                this.Content = relativeLayout;

                btnAssinar.Clicked += OnAssinar;
            }
            else
            {
                this.Content = browser;
            }

            GetXSLT().ContinueWith(action =>
            {
                if(action.IsCompleted && !action.IsFaulted)
                {
                    LoadHTML(html, document, action.Result);
                }
            });

            CreateInfoToolbarItem(this);
        }

        private void LoadHTML(HtmlWebViewSource html, Document document, string xslt)
        {
			if (document.Content != null)
			{
				string text = Encoding.UTF8.GetString(document.Content, 0, document.Content.Length);
				string docHtml = XmlHelper.Transform(xslt, text);

				if (Device.OS != TargetPlatform.iOS)
				{
					//Existe um bug no WebView do iOS: https://developer.xamarin.com/guides/xamarin-forms/working-with/webview/
					html.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
					html.Html = docHtml;
				}
				else
				{
					//iOS requires this action on the main UI thread
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        html.Html = docHtml;
                    });
				}
            }
        }

        private void CreateInfoToolbarItem(ContentPage page)
        {
            ToolbarItem ti = new ToolbarItem();
            ti.Priority = 0;
            ti.Order = ToolbarItemOrder.Primary;
            ti.Icon = (FileImageSource)FileImageSource.FromFile("question.png");
            ti.Command = new Command(() =>
            {
                page.Navigation.PushAsync(new DocumentInfo(request));
            });

            page.ToolbarItems.Add(ti);
        }

        private static string Xslt = null;

        private async Task<string> GetXSLT()
        {
            using(var progress = DialogHelper.ShowProgress(AppResources.LOADING_DOCUMENT))
            {
                if (Xslt == null)
                {
                    try
                    {
                        Xslt = await HttpRequest.GetStr(EndPointHelper.GetXSLT());
                    }
                    catch (Exception e)
                    {
                        //caso não seja possível obter o xslt, utiliza a versão embutida na app.
                        var localFileLoader = DependencyService.Get<IResourceLoader>();
                        using (Stream streamXslt = localFileLoader.OpenFile("documento.xsl"))
                        {
                            Xslt = StreamHelper.StreamToString(streamXslt);
                        }
                    }
                }
                return Xslt;
            }
        }
    }
}
