using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Dpwork.Core.Utils
{
    public class SerializeUtil
    {
        private static readonly JsonSerializerOptions _options;

        static SerializeUtil()
        {
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                //Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public static JsonSerializerOptions Options => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            //Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string Serialize(object obj, string defaultValue = "", JsonSerializerOptions options = null)
        {
            if (obj == null) return defaultValue;

            return JsonSerializer.Serialize(obj, obj.GetType(), options ?? _options);
        }

        public static async Task<string> SerializeAsync(object value, string defaultValue = "", JsonSerializerOptions options = null)
        {
            if (value == null) return defaultValue;
            using MemoryStream ms = new MemoryStream();

            await JsonSerializer.SerializeAsync(ms, value.GetType(), options ?? _options);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static T Deserialize<T>(string json, T defaultValue = default(T), JsonSerializerOptions options = null)
        {
            if (string.IsNullOrEmpty(json)) return defaultValue;
            return JsonSerializer.Deserialize<T>(json, options ?? _options);
        }

        public static object Deserialize(string json, Type returnType, JsonSerializerOptions options = null)
        {
            if (string.IsNullOrEmpty(json)) return default;
            return JsonSerializer.Deserialize(json, returnType, options ?? _options);
        }

        public static async Task<T> DeserializeAsync<T>(string json, T defaultValue = default(T), JsonSerializerOptions options = null)
        {
            if (string.IsNullOrEmpty(json)) return defaultValue;
            using MemoryStream ms = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(json);
            await ms.WriteAsync(bytes);

            return await JsonSerializer.DeserializeAsync<T>(ms, options ?? _options);
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

    //public class NumberToStringJsonConverter : JsonConverter<object>
    //{
    //    //
    //    // 摘要:
    //    //     Determines whether the specified type can be converted.
    //    //
    //    // 参数:
    //    //   typeToConvert:
    //    //     The type to compare against.
    //    //
    //    // 返回结果:
    //    //     true if the type can be converted; otherwise, false.
    //    public override bool CanConvert(Type typeToConvert)
    //    {
    //        return typeof(string) == typeToConvert || typeof(long) == typeToConvert || typeof(int) == typeToConvert;
    //    }

    //    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        if (reader.TokenType == JsonTokenType.Number)
    //        {
    //            return reader.TryGetInt64(out long v) ? v : reader.GetDouble().ToString();
    //        }
    //        if (reader.TokenType == JsonTokenType.String)
    //        {
    //            return reader.GetString();
    //        }
    //        using var doc = JsonDocument.ParseValue(ref reader);
    //        return doc.RootElement.Clone().ToString();
    //    }

    //    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStringValue(value.ToString());
    //    }
    //}
}
