using Acr.UserDialogs;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Services;
using PCLStorage;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Pages.PDF
{
    public class PdfPage: ContentPage
    {
        private ShareViewModel PdfShareViewModel = new ShareViewModel();
        
        private Document Document;

        private BaseInboxMessage SignatureRequest;

        private CustomWebView pdfWebView;

        private INativeFileSystem fileSystem = DependencyService.Get<INativeFileSystem>();

        private bool pdfLoaded = false;

        public PdfPage(BaseInboxMessage request, Document document, bool showSignButton, EventHandler OnAssinar)
        {
            this.Document = document;
            this.SignatureRequest = request;

            this.Title = "PDF";

            if (Device.OS == TargetPlatform.iOS)
                this.Icon = (FileImageSource)FileImageSource.FromFile("pdf.png");

            pdfWebView = new CustomWebView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MinimumHeightRequest = 300,
                OverviewMode = true
            };

            if (showSignButton)
            {
                var relativeLayout = new RelativeLayout();

                relativeLayout.Children.Add(pdfWebView, Constraint.RelativeToParent((parent) => {
                    return parent.X;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Y;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Height - 50;
                }));

                Button btnAssinar = new Button()
                {
                    Text = AppResources.SIGN_DOCUMENT,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                relativeLayout.Children.Add(btnAssinar, Constraint.RelativeToView(pdfWebView, (Parent, sibling) => {
                    return 0;
                }), Constraint.RelativeToView(pdfWebView, (parent, sibling) => {
                    return sibling.Height;
                }), Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }), Constraint.Constant(50));

                this.Content = relativeLayout;

                btnAssinar.Clicked += OnAssinar;
            }
            else
            {
                this.Content = pdfWebView;
            }

            CreateShareToolbarItem(this);
        }

        public void LoadPDF()
        {
            if(pdfLoaded)
                return;

            if (Document.PdfContent != null)
            {
                ShowPDF();
            }
            else
            {
                var progress = DialogHelper.ShowProgress(AppResources.LOADING_PDF);
                Document.LoadPdfDocument().ContinueWith(t =>
                {
                    if (t.IsCompleted && !t.IsFaulted)
                    {
                        ShowPDF();
                    }
                    else
                    {
                        DialogHelper.ShowAlertOK(AppResources.APP_TITLE, AppResources.FAILED_TO_LOAD_PDF);
                    }
                    progress.Dispose();
                });
            }
        }

        private void ShowPDF()
        {
            string pdfFolder = GetPDFDir(this.SignatureRequest);
            string fileName = fileSystem.JoinPaths(pdfFolder, "document.pdf");
            fileSystem.CreateFolder(pdfFolder);

            using (var stream = fileSystem.GetFileStream(fileName))
            {
                stream.Write(Document.PdfContent, 0, Document.PdfContent.Length);
                stream.Flush();
            }

            HtmlWebViewSource html = new HtmlWebViewSource();

            if (Device.OS != TargetPlatform.iOS)
            {
                //Existe um bug no WebView do iOS: https://developer.xamarin.com/guides/xamarin-forms/working-with/webview/
                html.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            }

            string pageSource = String.Format(
                @"<html>
	                        <head>
		                        <script type = 'text/javascript'>
			                        var url = 'file://{0}';
		                        </script>
		                        <script type='text/javascript' src='compatibility.js'></script>
	                        </head>
	                        <body>
		                        <div id='pages'>
	                            </div>

	                            <script type='text/javascript' src='../build/pdf.js'></script>
	                            <script type='text/javascript' src='customview.js'></script>
	                        </body>
	                        </html>", fileName);

            try
            {
                if (Device.OS != TargetPlatform.iOS)
                {
                    html.Html = pageSource;
                    pdfWebView.Source = html;
                }
                else
                {
                    //iOS requires this action on the main UI thread
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        html.Html = pageSource;
                        pdfWebView.Source = html;
                    });
                }

                //url.Url = DependencyService.Get<IBaseUrl>().Get() + "index.html"; //?file=" + WebUtility.UrlEncode(fileName.Result);
                PdfShareViewModel.FileName = fileName;
                PdfShareViewModel.MimeType = "application/pdf";
                pdfLoaded = true;
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message, AppResources.APP_TITLE, AppResources.OK);
            }
        }

        public string GetPDFDir(BaseInboxMessage req)
        {
            string cacheDir = fileSystem.GetFilesDir();
            string attPath = fileSystem.JoinPaths(cacheDir, "reqs", req.Id.ToString(), "pdf");
            fileSystem.CreateFolder(attPath);
            return attPath;
        }

        private void CreateShareToolbarItem(ContentPage page)
        {
            ToolbarItem ti = new ToolbarItem();
            ti.Priority = 0;
            ti.Order = ToolbarItemOrder.Primary;
            ti.Icon = (FileImageSource)FileImageSource.FromFile("share.png");
            ti.Command = PdfShareViewModel.ShareCommand;

            page.ToolbarItems.Add(ti);
        }
    }
}
