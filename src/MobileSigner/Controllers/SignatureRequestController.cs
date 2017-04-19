using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Model.Inbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Controllers
{
    public class SignatureRequestController
    {
        public async Task<bool> MarkAsRead(SignatureRequest signatureRequest)
        {
            try
            {
                var response = await HttpRequest.Post(EndPointHelper.GetReadSignatureRequestsURL(signatureRequest.MessageId));
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> MarkAsArchived(SignatureRequest signatureRequest)
        {
            try
            {
                var response = await HttpRequest.Post(EndPointHelper.GetArchiveSignatureRequestsURL(signatureRequest.MessageId));
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<SignatureRequestInboxPage> GetSignatureRequests(string source, bool archived, int page, int tamanhoPagina)
        {
            var inbox = await HttpRequest.GetJson<SignatureRequestsInbox>(EndPointHelper.GetSignatureRequestsURL(source, archived, page, tamanhoPagina));
            return new SignatureRequestInboxPage(inbox);
        }

        public async Task<List<SignedDocument>> GetSignedDocuments(int page, int tamanhoPagina)
        {
            var inbox = await HttpRequest.GetJson<List<UserInboxMessage<InboxMessageSignedDocument>>>(EndPointHelper.GetSignedDocumentsURL(page, tamanhoPagina));
            var signatureRequests = inbox.Select(m => new SignedDocument(m)).ToList();
            return signatureRequests;
        }
    }
}
