using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;

using System.Text.RegularExpressions;
using System.Collections;


namespace Experiment
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://www.baidu.com/s?wd=汽车知乎&rn= " + 10 + "";
            string html = search(url, "utf-8");
            List<Keyword> K = FindTitleAndLink(html);
            for(int i=0;i<K.Count;i++)
            {
                Console.WriteLine(K[i].ID+"     "+ K[i].Title + "   " + K[i].Link);
            }
        }
        public static string DownLoad(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);
                return html;
            }
            catch (Exception e)
            {
                Console.WriteLine("下载失败" + e.Message);
                return "";
            }
        }
        static List<Keyword> FindTitleAndLink(String html)
        {
            List<string> title = new List<string>();
            List<string> link = new List<string>();
            List<Keyword> keyword = new List<Keyword>();
            string p = "http://www.baidu.com/link?([^\"]*)\"";//不要第一个后边都要
            MatchCollection matches = new Regex(p).Matches(html);
            p= "\"title\":\"([^\"]*)\"";//全都要
            MatchCollection matches1 = new Regex(p).Matches(html);
            for (int i=1;i<matches.Count;i++)
            {
                if(!link.Contains(matches[i].ToString().Replace("\"", "").Trim()))
                {
                    link.Add(matches[i].ToString().Replace("\"","").Trim());
                }
                
            }
            for (int i = 0; i < matches1.Count; i++)
            {
                if (!title.Contains(matches1[i].ToString()))
                {
                    title.Add(matches1[i].ToString());
                }
            }
            for(int i=0;i<title.Count;i++)
            {
                keyword.Add(new Keyword { ID = i + 1, Title = title[i], Link = link[i] });
            }
            return keyword;
        }


        public static string search(string url, string Chareset)
        {
            HttpState result = new HttpState();
            Uri uri = new Uri(url);
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myHttpWebRequest.UseDefaultCredentials = true;
            myHttpWebRequest.ContentType = "text/html";
            myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50215;)";
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.CookieContainer = new CookieContainer();

            try
            {
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                // 从 ResponseStream 中读取HTML源码并格式化 add by cqp
                result.Html = readResponseStream(response, Chareset);
                result.CookieContainer = myHttpWebRequest.CookieContainer;
                return result.Html;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        public static string readResponseStream(HttpWebResponse response, string Chareset)
        {
            string result = "";
            using (StreamReader responseReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(Chareset)))
            {
                result = formatHTML(responseReader.ReadToEnd());
              
            }
            return result;
        }
        /// <summary>
        /// 描述:格式化网页源码
        /// 
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string formatHTML(string htmlContent)
        {
            string result = "";

            result = htmlContent.Replace("»", "").Replace(" ", "")
                  .Replace("©", "").Replace("/r", "").Replace("/t", "")
                  .Replace("/n", "").Replace("&", "&");
            return result;
        }
       
        
    }
    class HttpState
    {
        private string _statusDescription;

        public string StatusDescription
        {
            get { return _statusDescription; }
            set { _statusDescription = value; }
        }

        /// <summary>
        /// 回调 址址, 登陆测试中使用
        /// </summary>
        private string _callBackUrl;

        public string CallBackUrl
        {
            get { return _callBackUrl; }
            set { _callBackUrl = value; }
        }


        /// <summary>
        /// 网页网址 绝对路径格式
        /// </summary>
        private string _url;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 字符串的形式的Cookie信息
        /// </summary>
        private string _cookies;

        public string Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }

        /// <summary>
        /// Cookie信息
        /// </summary>
        private CookieContainer _cookieContainer = new CookieContainer();

        public CookieContainer CookieContainer
        {
            get { return _cookieContainer; }
            set { _cookieContainer = value; }
        }

        /// <summary>
        /// 网页源码
        /// </summary>
        private string _html;

        public string Html
        {
            get { return _html; }
            set { _html = value; }
        }

        /// <summary>
        /// 验证码临时文件(绝对路径)
        /// </summary>
        private string _tmpValCodePic;

        public string TmpValCodePic
        {
            get { return _tmpValCodePic; }
            set { _tmpValCodePic = value; }
        }

        /// <summary>
        /// 验证码临时文件名(相对路径)
        /// </summary>
        private string _tmpValCodeFileName = "emptyPic.gif";

        public string TmpValCodeFileName
        {
            get { return _tmpValCodeFileName; }
            set { _tmpValCodeFileName = value; }
        }

        /// <summary>
        /// 有验证码
        /// </summary>
        private bool _isValCode;

        public bool IsValCode
        {
            get { return _isValCode; }
            set { _isValCode = value; }
        }

        /// <summary>
        /// 验证码URL
        /// </summary>
        private string _valCodeURL;

        public string ValCodeURL
        {
            get { return _valCodeURL; }
            set { _valCodeURL = value; }
        }

        /// <summary>
        /// 验证码识别后的值
        /// </summary>
        private string _valCodeValue;

        public string ValCodeValue
        {
            get { return _valCodeValue; }
            set { _valCodeValue = value; }
        }

        /// <summary>
        /// 其它参数
        /// </summary>
        private Hashtable _otherParams = new Hashtable();

        public Hashtable OtherParams
        {
            get { return _otherParams; }
            set { _otherParams = value; }
        }

        // 重复添加处理 add by fengcj  09/11/19 PM
        public void addOtherParam(object key, object value)
        {
            if (!this.OtherParams.ContainsKey(key))
                this.OtherParams.Add(key, value);
            else
            {
                this.OtherParams[key] = value;
            }
        }

        public void removeOtherParam(object key)
        {
            this.OtherParams.Remove(key);
        }

        public object getOtherParam(object key)
        {
            return this.OtherParams[key];
        }
    }
    class Keyword
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
