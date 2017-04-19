using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{
    public class Signature
    {
        [JsonProperty("idDocumento")]
        public string Id { get; set; }

        [JsonProperty("assinaturaDigital")]
        public string SignatureData { get; set; }
    }
}
