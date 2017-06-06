using Almg.MobileSigner.Model.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contém classes que mapeiam para os objetos retornados pelo servidor. A interface do aplicativo móvel
/// não utiliza diretamente esses objetos pois eles estão sujeitos a sofrer alterações.
/// </summary>
namespace Almg.MobileSigner.Model.Inbox
{
    public class SignatureRequestsInbox
    {
        [JsonProperty("numNaoLidasNaoArquivadas")]
        public int NotReadCount { get; set; }

        [JsonProperty("numNaoLidasArquivadas")]
        public int ArchivedNotReadCount { get; set; }

        [JsonProperty("mensagens")]
        public List<UserInboxMessage<InboxMessageSignatureRequest>> Messages { get; set; }
    }

    public class UserInboxMessage<T>
    {
        [JsonProperty("mensagem")]
        public InboxMessage<T> Message { get; set; }

        [JsonProperty("dataLeitura")]
        [JsonConverter(typeof(TimestampDateConverter))]
        public DateTime? ReadDate { get; set; }

        [JsonProperty("dataArquivamento")]
        [JsonConverter(typeof(TimestampDateConverter))]
        public DateTime? ArchivedDate { get; set; }

        [JsonProperty("acoes")]
        public List<InboxAction> Actions { get; set; }

        public bool Read
        {
            get
            {
                return ReadDate.HasValue;
            }
        }
    }

    public class InboxAction
    {
        public string Method { get; set; }
        [JsonProperty("nome")]
        public string Name { get; set; }
        public string Url { get; set; }
        [JsonProperty("mensagemConfirmacao")]
        public string ConfirmMessage { get; set; }
    }

    public class InboxMessage<T> 
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("descricaoObjeto")]
        public string Description { get; set; }

        [JsonProperty("idObjeto")]
        public long IdObjeto { get; set; }

        [JsonProperty("situacao")]
        public string Status { get; set; }

        [JsonProperty("descricaoResumida")]
        public string MessageDescription { get; set; }

        [JsonProperty("dataAtualizacao")]
        [JsonConverter(typeof(TimestampDateConverter))]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("json")]
        public T Properties { get; set; }
    }

    public class InboxMessageRequester
    {
        [JsonProperty("nome")]
        public string Name { get; set; }
    }

    public class InboxMessageDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("propriedades")]
        public InboxMessageDocumentProperties Properties { get; set; }
    }

    public class InboxMessageDocumentProperties
    {
        [JsonProperty("tituloResumido")]
        public string Title { get; set; }

        [JsonProperty("numeroAnexos")]
        public int AttachmentCount { get; set; }

        [JsonProperty("descricaoResumida")]
        public string Description { get; set; }
    }

    public class InboxMessageAuthor
    {
        [JsonProperty("nome")]
        public string Name { get; set; }

        [JsonProperty("setor")]
        public InboxMessageAuthorSector Sector { get; set; }
    }

    public class InboxMessageAuthorSector
    {
        [JsonProperty("nome")]
        public string Name { get; set; }
    }

    public class InboxMessageWithDocument
    {
        [JsonProperty("documentos")]
        public List<InboxMessageDocument> Documents { get; set; }
    }

    public class InboxMessageSignatureRequest: InboxMessageWithDocument
    {
        [JsonProperty("solicitante")]
        public InboxMessageRequester Requester { get; set; }

        [JsonProperty("dataSolicitacao")]
        [JsonConverter(typeof(InboxRequestDateConverter))]
        public DateTime RequestDate { get; set; }
    }

    public class InboxMessageSignedDocument: InboxMessageWithDocument
    {
        [JsonProperty("autor")]
        public InboxMessageAuthor Author { get; set; }

        [JsonProperty("descricaoResumida")]
        public string Description { get; set; }

        [JsonProperty("tituloResumido")]
        public string Title { get; set; }
    }
}
