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
namespace 蓝图重制版.BluePrint.INode
{
    public class DES1
    {
        /// <summary>
        ///  DES 解密
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="key">密匙</param>
        /// <returns>明文</returns>
        public static string DesDecrypt(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return "";
                var imgData = str;//.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
                Byte[] toEncryptArray = Convert.FromBase64String(imgData);
                DES rm = DES.Create();
                {
                    rm.Mode = CipherMode.CBC;
                    rm.Padding = PaddingMode.PKCS7;
                    rm.Key = Encoding.UTF8.GetBytes("12345678");
                    rm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                };
                ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        ///  DES 加密
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="key">密匙</param>
        /// <returns>明文</returns>
        public static string DESEncrypt(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return "";
                var imgData = str;//.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
                Byte[] toEncryptArray = Encoding.UTF8.GetBytes(imgData);
                DES rm = DES.Create();
                {
                    rm.Mode = CipherMode.CBC;
                    rm.Padding = PaddingMode.PKCS7;
                    rm.Key = Encoding.UTF8.GetBytes("12345678");
                    rm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                };
                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
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
            a = new Random(System.DateTime.Now.Millisecond);
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }

            GTime = DateTime.Now;
            var time = GetNetworkTime();
            if (time!=null)
            {
                GTime = (DateTime)time;
            }
        }
        public static string HostOpgg = "http://121.36.203.245:8682";

        public static DateTime GTime;
        static System.Random a;
        public static string time_id = LOL_JSON.GetTimeStamp();
        /// <summary>
        /// 到期时间 默认是到期的
        /// </summary>
        public static DateTime ExpireTime = new DateTime(2021, 7, 5).Add(TimeSpan.FromDays(30));
        public static void Log(string data) {
            File.AppendAllText($"Log/日志_{time_id}", $"{data}\r\n");
        }


        /// <summary>
        /// 是否到期
        /// </summary>
        /// <param name="joinDate">签约时间</param>
        /// <param name="duration">签约时长</param>
        /// <returns></returns>
        public static bool IsExV()
        {
            //2022-7-13   2022-7-15
            return ExpireTime <= GTime;
            //return GTime - ExpireTime > TimeSpan.FromDays(30);
        }
        /// <summary>
        /// 根据签约时间和签约时长来判断是否到期
        /// </summary>
        /// <param name="joinDate">签约时间</param>
        /// <param name="duration">签约时长</param>
        /// <returns></returns>
        public static bool IsExpriredByDay(DateTime joinDate, double duration)
        {
            return GTime - joinDate > TimeSpan.FromDays(duration);
        }
        /// <summary>
        /// 是否到期
        /// </summary>
        /// <param name="joinDate">签约时间</param>
        /// <param name="duration">签约时长</param>
        /// <returns></returns>
        public static bool IsEx()
        {
            return IsExpriredByDay(new DateTime(2023,4,20),100);
        }

        
        public static DateTime? GetNetworkTime()
        {
            //default Windows time server
            try
            {
                const string ntpServer = "time.windows.com";

                // NTP message size - 16 bytes of the digest (RFC 2030)
                var ntpData = new byte[48];

                //Setting the Leap Indicator, Version Number and Mode values
                ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;

                //The UDP port number assigned to NTP is 123
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                //NTP uses UDP

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.Connect(ipEndPoint);

                    //Stops code hang if NTP is blocked
                    socket.ReceiveTimeout = 3000;

                    socket.Send(ntpData);
                    socket.Receive(ntpData);
                    socket.Close();
                }

                //Offset to get to the "Transmit Timestamp" field (time at which the reply 
                //departed the server for the client, in 64-bit timestamp format."
                const byte serverReplyTime = 40;

                //Get the seconds part
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

                //Get the seconds fraction
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                //Convert From big-endian to little-endian
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                //**UTC** time
                var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

                return networkDateTime.ToLocalTime();
            }
            catch (Exception e)
            {
                return null;
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
        public static System.Random GetRandom() {
            return a;
        }

        public static string ToPos(this string data)
        {
            switch (data)
            {
                case "mid":
                    return "中单";
                case "top":
                    return "上单";
                case "adc":
                    return "ADC";
                case "jungle":
                    return "打野";
                case "support":
                    return "辅助";
                default:
                    return "";
            }
        }
        /*public static object ToJson(this string _input)
        {
            try
            {
                var ret = JsonConvert.DeserializeObject(_input);
                return ret;
            }
            catch (Exception)
            {
                return new object();
            }
        }*/
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
            var list = url.Split("/");
            if (list.Length > 0 && Directory.Exists(name))
            {
                var path = Path.Join(System.Environment.CurrentDirectory, name, list[list.Length - 1]);
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

            //return System.Text.Json.JsonSerializer.Deserialize<T>(_input);
        }
        /*public static HtmlNodeCollection ToDom(this string text, string ur)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(text);
            HtmlNodeCollection nove = doc.DocumentNode.SelectNodes(ur);
            return nove;
        }*/

        //
        /*public static JsonElement Get(this JsonElement _input,string name)
        {
            return _input.GetProperty(name);
        }//*/
    }
}
