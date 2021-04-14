using Dpwork.Core.Utils;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dpwork.Core.Extensions
{
    public static class HttpClientExtension
    {
        public static async Task<string> PostAsync(this HttpClient client, string reqUrl, object obj, string contentType = null)
        {
            var postData = StringUtil.JsonUrlEncode(SerializeUtil.Serialize(obj));
            using HttpContent httpContent = new StringContent(postData, Encoding.UTF8);
            if (contentType != null)
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            HttpResponseMessage response = await client.PostAsync(reqUrl, httpContent);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
