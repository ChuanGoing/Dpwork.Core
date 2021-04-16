using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Dpwork.Core.Utils
{
    public class SerializeUtil
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            //Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string Serialize(object obj, string defaultValue = "", JsonSerializerOptions options = null)
        {
            if (obj == null) return defaultValue;

            return JsonSerializer.Serialize(obj, obj.GetType(), options ?? Options);
        }

        public static async Task<string> SerializeAsync(object value, string defaultValue = "", JsonSerializerOptions options = null)
        {
            if (value == null) return defaultValue;
            using MemoryStream ms = new MemoryStream();

            await JsonSerializer.SerializeAsync(ms, value.GetType(), options ?? Options);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static T Deserialize<T>(string json, T defaultValue = default(T), JsonSerializerOptions options = null)
        {
            if (string.IsNullOrEmpty(json)) return defaultValue;
            return JsonSerializer.Deserialize<T>(json, options ?? Options);
        }

        public static object Deserialize(string json, Type returnType, JsonSerializerOptions options = null)
        {
            if (string.IsNullOrEmpty(json)) return default;
            return JsonSerializer.Deserialize(json, returnType, options ?? Options);
        }

        public static async Task<T> DeserializeAsync<T>(string json, T defaultValue = default(T), JsonSerializerOptions options = null)
        {
            if (string.IsNullOrEmpty(json)) return defaultValue;
            using MemoryStream ms = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(json);
            await ms.WriteAsync(bytes);

            return await JsonSerializer.DeserializeAsync<T>(ms, options ?? Options);
        }

        public static IEnumerable<T> JsonToArry<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) return default;
            DataContractJsonSerializer _Json = new DataContractJsonSerializer(typeof(IEnumerable<T>));
            byte[] _Using = Encoding.UTF8.GetBytes(json);
            using MemoryStream _MemoryStream = new MemoryStream(_Using)
            {
                Position = 0
            };
            return (IEnumerable<T>)_Json.ReadObject(_MemoryStream);
        }
    }
}
