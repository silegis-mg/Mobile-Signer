using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{
    public class SignatureStatus
    {
        [JsonProperty("descricao")]
        public String Description { get; set; }
    }
}
