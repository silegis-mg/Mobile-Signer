using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Pages.PDF;
using Almg.MobileSigner.Pages.Signature;
using Almg.MobileSigner.Resources;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Pages
{
    public class SignaturePage : TabbedPage
    {
        private BaseInboxMessage SignatureRequest;
        private Document Document;

        public Task<bool> PageClosedTask {
            get {
                return tcs.Task;
            }
        }

        private TaskCompletionSource<bool> tcs { get; set; }

        public SignaturePage(string title, BaseInboxMessage request, Document document, bool showSignButton = true)
        {
            this.tcs = new TaskCompletionSource<bool>();
            this.SignatureRequest = request;
            this.Document = document;

            this.Title = title;
            this.CurrentPageChanged += AssinarPage_CurrentPageChanged;

            this.Children.Add(new TextPage(request, document, showSignButton, Sign));
            this.Children.Add(new PdfPage(request, Document, showSignButton, Sign));
        }

        protected override void OnDisappearing()
        {
            if (!tcs.Task.IsCompleted)
                tcs.TrySetResult(false);
            base.OnDisappearing();
        }

        private void AssinarPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if(sender is TabbedPage)
            {
                TabbedPage tabbedPage = (sender as TabbedPage);

                if(tabbedPage.CurrentPage is PdfPage)
                {
                    (tabbedPage.CurrentPage as PdfPage).LoadPDF();
                }
            }
        }
        
        private async void Sign(object sender, EventArgs e)
        {
            try
            {
                await SignerHelper.Sign(this.SignatureRequest);
                tcs.SetResult(true);
                await Navigation.PopAsync();
            } catch (DigitalSignatureError ex) {
                DialogHelper.ShowMessage(ex.Message);
            } catch (TaskCanceledException) {
                DialogHelper.ShowMessage(AppResources.SIGNATURE_CANCELLED);
            } catch (Exception ex1) {
                //TODO: log this unexpected error
                DialogHelper.ShowMessage(ex1.Message);
            }
        }
    }
}
