using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model.Inbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{
    /// <summary>
    /// A document that will be signed. 
    /// Right now only XML documents are supported.
    /// </summary>
    public class Document
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public List<string> AttachmentsUrls { get; set; }

        public List<Author> Authors { get; set; }

        public byte[] Content { get; set; }

        public byte[] PdfContent { get; set; }

        public int? AttachmentCount { get; set; }

        public Document()
        {

        }

        public Document(InboxMessageDocument document)
        {
            this.Id = document.Id;
            this.Url = document.Url;
            if(document.Properties!=null)
            {
                this.Description = document.Properties.Description;
                this.Title = document.Properties.Title;
                this.AttachmentCount = document.Properties.AttachmentCount;
            }
        }

        public bool XmlLoaded
        {
            get
            {
                return Content != null;
            }
        }

        public async Task LoadXmlDocument()
        {
            if (Content == null)
            {
                Content = await HttpRequest.GetByteArray(EndPointHelper.GetDocumentXML(Id));
            }
        }

        public async Task LoadPdfDocument()
        {
            if(PdfContent == null)
            {
                PdfContent = await HttpRequest.GetByteArray(EndPointHelper.GetDocumentPDF(Id));
            }
        }
    }
}
