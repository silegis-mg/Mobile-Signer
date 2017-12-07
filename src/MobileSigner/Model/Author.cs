using Newtonsoft.Json;

namespace Almg.MobileSigner.Model
{
    public class Author
    {
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Name { get; set; }

        [JsonProperty("situacaoAssinatura")]
        public SignatureStatus Status { get; set; }
    }
}