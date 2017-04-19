using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model.Converters
{
    /// <summary>
    /// Converte a data no formato dd/mm/yyyy HH:mm da caixa de entrada para um tipo DateTime
    /// </summary>
    public class InboxRequestDateConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.Value != null)
                {
                    return DateTime.ParseExact(reader.Value.ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                else
                {
                    return null;
                }
            } catch(Exception e)
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("dd/MM/yyyy"));
        }
    }
}
