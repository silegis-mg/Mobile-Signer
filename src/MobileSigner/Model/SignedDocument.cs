using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model.Inbox;
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
    public class SignedDocument : BaseInboxMessage
    {
        public SignedDocument(): base()
        {
        }

        public SignedDocument(UserInboxMessage<InboxMessageSignedDocument> message): this()
        {
            this.Id = message.Message.IdObjeto;
            this.MessageId = message.Message.Id;
            this.Title = message.Message.Description;
            this.Status = message.Message.Status;
            this.Read = message.Read;

            this.Date = this.LastUpdate = message.Message.LastUpdate;
            this.Author = message.Message.Properties.Author?.Sector?.Name;

            //mensagens resumidas para celular
            if(!string.IsNullOrEmpty(message.Message.Properties.Description))
            {
                this.Description = HtmlHelper.ToPlainText(message.Message.Properties.Description);
            }

            if(!string.IsNullOrEmpty(message.Message.Properties.Title))
            {
                this.Title = message.Message.Properties.Title;
            }

            this.Documents = message.Message.Properties.Documents.Select(d => new Document(d)).ToList();
            this.Actions = message.Actions.Select(a => new InboxMessageAction(a)).ToList();
        }

        public override string ToString()
        {
            return this.Title + ": " + this.Description;
        }
    }
}
