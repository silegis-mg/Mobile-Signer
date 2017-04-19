using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model.Inbox;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{
    /// <summary>
    /// A signature request that contains details about the document to be digitally signed.
    /// </summary>
    public class SignatureRequest: BaseInboxMessage
    {
        public SignatureRequest(): base()
		{
		}

        public SignatureRequest(UserInboxMessage<InboxMessageSignatureRequest> message): this()
        {
            this.Id = message.Message.IdObjeto;
            this.MessageId = message.Message.Id;
            this.Title = message.Message.Description;
            this.Status = message.Message.Status;
            this.Read = message.Read;

            this.Date = message.Message.Properties.RequestDate;
            this.LastUpdate = message.Message.LastUpdate;
            this.Author = message.Message.Properties.Requester?.Name;
            this.Documents = message.Message.Properties.Documents.Select(d => new Document(d)).ToList();

            this.Archived = message.ArchivedDate != null;

            if (message.Actions!=null)
            {
                this.Actions = message.Actions.Select(a => new InboxMessageAction(a)).ToList();
            }

            if (Documents.Count()==1)
            {
                this.Description = HtmlHelper.ToPlainText(Documents.First().Description);
            } else
            {
                this.Description = "";
            }
        }

        public override string ToString()
        {
            return this.Title + ": " + this.Description;
        }

        public bool CanBeSigned
        {
            get
            {
                return this.Actions.Exists(a => a.Name == "ASSINAR");
            }
        }
    }
}
