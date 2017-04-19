using Almg.MobileSigner.Model.Inbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{
    public class SignatureRequestInboxPage
    {
        public List<SignatureRequest> Requests { get; set; }
        public int NotReadCount { get; set; }
        public int NotReadCountArchived { get; set; }

        public SignatureRequestInboxPage(SignatureRequestsInbox inboxResponse)
        {
            try
            {
                this.Requests = inboxResponse.Messages.Select(m => new SignatureRequest(m)).ToList();
                this.NotReadCount = inboxResponse.NotReadCount;
                this.NotReadCountArchived = inboxResponse.ArchivedNotReadCount;
            } catch(Exception e)
            {
                throw e;
            }
        }
    }
}
