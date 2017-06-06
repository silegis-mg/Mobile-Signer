using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Almg.MobileSigner.Pages
{
    public partial class DocumentInfo : ContentPage
    {
        private AuthorViewModelCollection autores = new AuthorViewModelCollection();

        class AuthorViewModel
        {
            public AuthorViewModel(Author author)
            {
                this.Name = author.Name;
                this.Signed = author.Status?.Description;
            }

            public string Name { get; set; }
            public string Signed { get; set; }
        }

        class AuthorViewModelCollection : List<AuthorViewModel>, INotifyCollectionChanged
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void ReplaceItems(List<AuthorViewModel> requests)
            {
                this.Clear();
                this.AddRange(requests);
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public DocumentInfo(BaseInboxMessage signatureRequest)
        {
            InitializeComponent();

            GetAuthors(signatureRequest.Documents.First()).ContinueWith(t =>
            {
                if(!t.IsFaulted)
                {
                    autores.ReplaceItems(t.Result);
                }
            });

            lstAutores.ItemsSource = autores;

            lblCreator.Text = AppResources.CREATOR;
            lblCreatorValue.Text = signatureRequest.Author;

            lblTitle.Text = signatureRequest.Title;
            lblDescription.Text = signatureRequest.Description;

            lblDateTime.Text = AppResources.CREATION_TIME;
            lblDateTimeValue.Text = signatureRequest.Date.ToString("dd/MM/yyyy HH:mm");

            lblAuthors.Text = AppResources.AUTHORS;

            lstAutores.ItemTapped += LstAutores_ItemTapped;
        }

        private void LstAutores_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstAutores.SelectedItem = null;
        }

        private async Task<List<AuthorViewModel>> GetAuthors(Document doc)
        {
            using(var progress = DialogHelper.ShowProgress(AppResources.LOADING_DOCUMENT_INFO))
            {
                var autores = await HttpRequest.GetJson<List<Author>>(EndPointHelper.GetDocumentAuthors(doc.Id));
                return autores.Select(a => new AuthorViewModel(a)).ToList();
            }
        }
    }
}
