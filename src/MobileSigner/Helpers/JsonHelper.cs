using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Almg.MobileSigner.Helpers
{
	public static class JsonHelper
    {
        public static T FromJsonStream<T>(this Stream stream)
        {
            JsonSerializer serializer = new JsonSerializer();
            T data;
            using (StreamReader streamReader = new StreamReader(stream))
            {
                data = (T)serializer.Deserialize(streamReader, typeof(T));
            }
            return data;
        }

        public static T FromJsonString<T>(this String json)
        {
            T data;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                data = FromJsonStream<T>(stream);
            }
            return data;
        }

        public static string ToJson<T>(T obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }
    }
}
