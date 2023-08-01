using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
namespace BluePrint.Core.INode
{
    public static class LOL_JSON
    {

        public static string TIPS = "\r\n内容前面加y代表需要转义,n代表不转义，y是yes n是no\r\n例如:需要转义\\r\\n就可以填y\\r\\n 不需要就n\\r\\n";
        public static string ListItemTIPS = "\r\n如果作为列表处理条件，默认就是列表每一项，如果有连接线，那就是上一个节点返回值";

        public static string ToLiteral(this string input)
        {
            if (input.StartsWith("y"))
            {
                return JsonConvert.SerializeObject(input.Remove(0, 1));
            }
            if (input.StartsWith("n"))
            {
                return $"\"{input.Remove(0, 1)}\"";
            }
            return JsonConvert.SerializeObject(input);
        }
        static LOL_JSON() {
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }

        }
       
        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string ReplaceHtmlTag(this string html,string title, int length = 0)
        {
            html = html.Replace("<br>","\r\n");
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return $"{title}\r\n{strText}";
        }
        public static string ToPath(this string url,string name)
        {
            var list = url.Split('/');
            if (list.Length > 0 && Directory.Exists(name))
            {
                var path = Path.Combine(System.Environment.CurrentDirectory, name, list[list.Length - 1]);
                return path;
            }
            else {
                return url;
            }
        }

        public static T ToJson<T>(this string _input)
        {
            try
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                //var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                //var json = JsonConvert.SerializeObject（data， Formatting.Indented， jsonSetting）;
                return JsonConvert.DeserializeObject<T>(_input, jsonSettings);
            }
            catch (Exception e)
            {
                return default(T);
            }
            
            //return System.Text.Json.JsonSerializer.Deserialize<T>(_input);
        }
        public static JObject ToJson(this string _input,int i = 0) {
            return JObject.Parse(_input);
        }
        public static object ToJson(this string _input)
        {
            try
            {
                //var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                //var json = JsonConvert.SerializeObject（data， Formatting.Indented， jsonSetting）;
                return JsonConvert.DeserializeObject(_input);
            }
            catch (Exception e)
            {
                return null;
            }

            //return System.Text.Json.JsonSerializer.Deserialize<T>(_input);
        }
        public static string ToJsonString(this object _input)
        {
            try
            {
                //var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                //var json = JsonConvert.SerializeObject（data， Formatting.Indented， jsonSetting）;
                return JsonConvert.SerializeObject(_input);
            }
            catch (Exception e)
            {
                return "{}";
            }
        }
    }
}
