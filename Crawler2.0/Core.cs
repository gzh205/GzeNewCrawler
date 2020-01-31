using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HtmlAgilityPack;
namespace Crawler2
{
    /// <summary>
    /// 一个网页页面对应着一个Core对象
    /// </summary>
    public class Core
    {
        /// <summary>
        /// 当前页面的网址
        /// </summary>
        public Uri thisUrl { get; set; }
        /// <summary>
        /// 当前页面内容的对象
        /// </summary>
        public HtmlAgilityPack.HtmlDocument webPage { get; protected set; }
        /// <summary>
        /// 当前页面中包含的所有有效超链接
        /// </summary>
        public Uri[] urlLists { get; protected set; }
        /// <summary>
        /// 该网页的深度
        /// </summary>
        public int depth { get; set; }
        public Core()
        {
        }
        /// <summary>
        /// 获取一个页面对象
        /// </summary>
        /// <param name="url">需打开页面的网址</param>
        /// <param name="depth">爬虫的爬取深度</param>
        /// <returns>如果成功打开网页，返回Core对象。若无法打开网页或超链接是无效的，返回null</returns>
        public static Core getCore(Uri url, int depth)
        {
            WebClient client = new WebClient();
            Core core = new Core();
            string str = null;
            try
            {
                str = client.DownloadString(url);
            }
            catch (WebException)
            {
                for(int i = 0; i < Crawler.RetryTime; i++)
                {
                    str = client.DownloadString(url);
                    if (str != null && str != "") break;
                }
            }
            core.thisUrl = url;
            core.webPage = new HtmlAgilityPack.HtmlDocument();
            if (str == null || str == "") return null;
            core.webPage.LoadHtml(str);
            core.depth = depth;
            return core;
        }
        /// <summary>
        /// 获取该页面中所有的超链接，并保存到urlLists成员中
        /// </summary>
        public void getUrls()
        {
            HtmlAgilityPack.HtmlNodeCollection nodes = webPage.DocumentNode.SelectNodes("//a/@href");
            if (nodes == null) return;
            List<Uri> list = new List<Uri>();
            foreach (HtmlNode n in nodes)
            {
                string str = n.GetAttributeValue("href", null);
                if (str != null)
                    list.Add(new Uri(thisUrl, str));
            }
            this.urlLists = list.ToArray();
        }
        /// <summary>
        /// 根据字符串提供的xpath获取匹配到的字符串
        /// </summary>
        /// <param name="str">xpath</param>
        /// <returns>字符串</returns>
        public string[] getXPath(string str)
        {
            HtmlAgilityPack.HtmlNodeCollection nodes = webPage.DocumentNode.SelectNodes(str);
            List<String> list = new List<string>();
            foreach (HtmlNode n in nodes)
            {
                string txt = n.GetDirectInnerText();
                if (txt != null || txt != "")
                    list.Add(txt);
            }
            return list.ToArray();
        }
        /// <summary>
        /// 判断网页中是否存在某个关键字
        /// </summary>
        /// <param name="str">关键字</param>
        /// <returns>页面中存在该关键字返回true，否则返回false</returns>
        public bool existString(string str)
        {
            return webPage.Text.Contains(str);
        }
    }
}
