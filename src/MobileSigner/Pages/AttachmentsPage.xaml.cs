using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Almg.MobileSigner.Pages
{
    class AttachmentsCollection : List<Attachment>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void ReplaceItems(List<Attachment> requests)
        {
            this.Clear();
            this.AddRange(requests);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    public partial class AttachmentsPage : ContentPage
    {
        private AttachmentsCollection attachments = new AttachmentsCollection();
        private INativeFileSystem fileSystem = DependencyService.Get<INativeFileSystem>();
        private BaseInboxMessage inboxMessage;

        public AttachmentsPage(BaseInboxMessage req)
        {
            InitializeComponent();
            this.Title = AppResources.ATTACHMENTS;
            this.inboxMessage = req;

            GetAttachments(req.Documents.First()).ContinueWith(t =>
            {
                if(!t.IsFaulted)
                {
                    attachments.ReplaceItems(t.Result);
                }
            });
            lstAttachments.ItemsSource = attachments;
            lstAttachments.ItemSelected += LstAttachments_ItemSelected;
        }

        private async void LstAttachments_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                    return;

                Attachment att = (Attachment)e.SelectedItem;
                lstAttachments.SelectedItem = null;

                string attDir = GetAttachmentDir(this.inboxMessage);
				string file = fileSystem.JoinPaths(attDir, FileHelper.SanitizeFileName(att.NomeArquivo));

                att.Loading = true;

                bool fileExists = fileSystem.FileExists(file);
                using (var stream = fileSystem.GetFileStream(file))
                {
                    if (!fileExists || stream.Length < att.Tamanho)
                    {
                        await HttpRequest.DownloadFileToStream(EndPointHelper.GetAbsoluteUrl(att.Url), stream);
                        stream.Flush();
                    }                
                }

				att.Loading = false;

                ShareViewModel share = new ShareViewModel();
                share.FileName = file;
                share.MimeType = att.MimeType;
                share.Share();

            } catch(Exception ex)
            {
                await DisplayAlert(AppResources.APP_TITLE, ex.Message, AppResources.OK);
            }
        }

        private async Task<List<Attachment>> GetAttachments(Document doc)
        {
            using (var progress = DialogHelper.ShowProgress(AppResources.LOADING_DOCUMENT_INFO))
            {
                var autores = await HttpRequest.GetJson<List<Attachment>>(EndPointHelper.GetDocumentAttachments(doc.Id));
                return autores;
            }
        }

        public string GetAttachmentDir(BaseInboxMessage req)
        {
            string cacheDir = fileSystem.GetFilesDir();
            string attPath = fileSystem.JoinPaths(cacheDir, "reqs", req.Id.ToString());
            fileSystem.CreateFolder(attPath);
            return attPath;
        }

        protected override void OnDisappearing()
        {
            
        }
    }
}
