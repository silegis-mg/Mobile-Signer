using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Almg.MobileSigner.Helpers
{
	public class EndPointHelper
	{
        public static string GetHost()
        {
            return Config.Host;
        }

		public static string GetBaseApiURL()
		{
            return Config.BaseUrl;
        }

		public static string GetCertificateURL()
		{
			return GetBaseApiURL() + "/api/v1/certificate/get";
		}

        public static string GetLoginTokenURL()
        {
            return GetBaseApiURL() + "/api/v1/login/token";
        }

        public static string GetLoginUserPassURL()
        {
            return GetBaseApiURL() + "/api/v1/login/userpass";
        }

        public static string GetSignatureRequestsURL(string source, bool archived, int page = 0, int tamanhoPagina = 20)
		{
            return GetBaseApiURL() + "/api/v1/solicitacoes-assinatura/inbox?origem=" + source + "&arquivadas=" + archived + "&pagina=" + page + "&tamanhoPagina=" + tamanhoPagina;
        }

        public static string GetSignedDocumentsURL(int page = 0, int tamanhoPagina = 20)
        {
            return GetBaseApiURL() + "/api/v1/inbox/DOCUMENTOS_ASSINADOS?" + "pagina=" + page + "&tamanhoPagina=" + tamanhoPagina + "&incluirAcoes=true";
        }

        public static string GetReadSignatureRequestsURL(long messageId)
        {
            return GetBaseApiURL() + "/api/v1/solicitacoes-assinatura/inbox/lida/" + messageId;
        }

        public static string GetArchiveSignatureRequestsURL(long messageId)
        {
            return GetBaseApiURL() + "/api/v1/solicitacoes-assinatura/inbox/arquivar/" + messageId;
        }

        public static string GetRemoveSignatureURL(List<string> uris)
        {
            var queryParams = uris.Select(id => "id=" + UrlHelper.UrlEncode(id)).Aggregate((a, b) => a + "&" + b);
            return GetBaseApiURL() + "/api/v1/documentos/assinatura?" + queryParams;
        }

        public static string GetCheckAppVersionUrl(TargetPlatform platform)
        {
            return GetBaseApiURL() + "/api/v1/get-app-version?platform=" + platform.ToString();
        }

        public static string GetDocumentXML(string id)
        {
            return GetBaseApiURL() + "/api/v1/documentos?id=" + id + "&formato=application%2Fxml";
        }

        public static string GetDocumentPDF(string id)
        {
            return GetBaseApiURL() + "/api/v1/documentos?id=" + id + "&formato=application%2Fpdf";
        }

        public static string GetDocumentAuthors(string id)
        {
            return GetBaseApiURL() + "/api/v1/documentos/autores?id=" + id;
        }
        
        public static string GetDocumentAttachments(string id)
        {
            return GetBaseApiURL() + "/api/v1/documentos/anexos?id=" + id;
        }

        public static string GetSendSignature()
        {
            return GetBaseApiURL() + "/api/v1/documentos/assinatura";
        }

        public static string GetNotificationTokenSubscribe()
        {
            return GetBaseApiURL() + "/api/v1/device/token";
        }

        public static string GetAbsoluteUrl(string url)
        {
            return GetBaseApiURL() + (url.StartsWith("/")?url:"/"+url);
        }
    }
}

