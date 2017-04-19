using Almg.MobileSigner.Resources.Icons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Model
{
    public class Attachment: INotifyPropertyChanged
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nome")]
        public string NomeArquivo { get; set; }

        [JsonProperty("mimetype")]
        public string MimeType { get; set; }

        private bool loading; 
		public bool Loading {
            get
            {
                return loading;
            }
            set
            {
                loading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Loading"));
            }
        }

        public ImageSource Image {
            get {
                return Icon.FromMimeType(MimeType);
            }
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        public Dictionary<string, object> Propriedades { get; set;}

        public string Tipo
        {
            get
            {
                if (Propriedades.ContainsKey("titulo"))
                    return (string)Propriedades["titulo"];
                else return NomeArquivo;
            }
        }

        public string Nome
        {
            get
            {
                if (Propriedades.ContainsKey("tipoAnexo"))
                    return ((Newtonsoft.Json.Linq.JObject)Propriedades["tipoAnexo"]).Value<string>("nome");
                else return null;
            }
        }

        public long Tamanho { get; set; }

        public Attachment()
        {
			Loading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
