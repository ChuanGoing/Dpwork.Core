using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dpwork.Core.Utils
{
    public class StringUtil
    {
        public static string GetJsonObject(string json, string key, bool children = false)
        {
            if (string.IsNullOrEmpty(json)) return string.Empty;

            using var jsojb = SerializeUtil.Deserialize<JsonDocument>(json);
            if (jsojb != null)
            {
                foreach (var item in jsojb.RootElement.EnumerateObject())
                {
                    if (item.Name == key)
                    {
                        return item.Value.ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// json转urlencode
        /// </summary>
        /// <returns></returns>
        public static string JsonUrlEncode(string json)
        {
            Dictionary<string, object> dic = SerializeUtil.Deserialize<Dictionary<string, object>>(json);
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, object> item in dic)
            {
                builder.Append(GetFormDataContent(item, ""));
            }
            return builder.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 递归转formdata
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preStr"></param>
        /// <returns></returns>
        private static string GetFormDataContent(KeyValuePair<string, object> item, string preStr)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(item.Value?.ToString()))
            {
                builder.AppendFormat("{0}={1}", string.IsNullOrEmpty(preStr) ? item.Key : (preStr + "[" + item.Key + "]"), System.Web.HttpUtility.UrlEncode((item.Value == null ? "" : item.Value.ToString()).ToString()));
                builder.Append("&");
            }
            else
            {
                //如果是数组
                if (item.Value.GetType().Name.Equals("JArray"))
                {
                    var children = SerializeUtil.Deserialize<List<object>>(item.Value.ToString());
                    for (int j = 0; j < children.Count; j++)
                    {
                        Dictionary<string, object> childrendic = SerializeUtil.Deserialize<Dictionary<string, object>>(SerializeUtil.Serialize(children[j]));
                        foreach (var row in childrendic)
                        {
                            builder.Append(GetFormDataContent(row, string.IsNullOrEmpty(preStr) ? (item.Key + "[" + j + "]") : (preStr + "[" + item.Key + "][" + j + "]")));
                        }
                    }

                }
                //如果是对象
                else if (item.Value.GetType().Name.Equals("JObject"))
                {
                    Dictionary<string, object> children = SerializeUtil.Deserialize<Dictionary<string, object>>(item.Value.ToString());
                    foreach (var row in children)
                    {
                        builder.Append(GetFormDataContent(row, string.IsNullOrEmpty(preStr) ? item.Key : (preStr + "[" + item.Key + "]")));
                    }
                }
                //字符串、数字等
                else
                {
                    builder.AppendFormat("{0}={1}", string.IsNullOrEmpty(preStr) ? item.Key : (preStr + "[" + item.Key + "]"), System.Web.HttpUtility.UrlEncode((item.Value == null ? "" : item.Value.ToString()).ToString()));
                    builder.Append("&");
                }
            }

            return builder.ToString();
        }

        public static string ObjectToString(params object[] input)
        {
            var result = string.Empty;
            foreach (var item in input)
            {
                if (item == null) continue;
                result += SerializeUtil.Serialize(item) + "_";
            }

            result = result.TrimEnd('_');

            return result;
        }
    }
}
