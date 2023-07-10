using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server
{
    public class Request
    {
        /// <summary>
        /// 请求头部
        /// </summary>
        public Dictionary<string, string> RequestHeader = new Dictionary<string, string>();
        public Dictionary<string, string> RequestParameters = new Dictionary<string, string>();
        public Request(string request)
        {
            if (request == null || request == "")
                throw new Exceptions.InvalidRequestException("请求字符串是空的");
            //分离请求头部和请求参数
            string[] datas = request.Trim('\0').Split(new string[]{ "\r\n\r\n"}, StringSplitOptions.None);
            string[] reqs = datas[0].Split('\n');
            string[] t = reqs[0].Trim('\r',' ').Split(' ');
            if (t.Length != 3)
                throw new Exceptions.InvalidRequestException("没有请求方式，URL和版本号中的一个");
            RequestHeader.Add("method", t[0]);
            if (RequestHeader["method"] == "GET")
            { 
                //获取URL后面的GET参数
                string[] get_param = t[1].Split('?');
                if (get_param.Length > 1)
                {
                    get_param[1] = System.Web.HttpUtility.UrlDecode(get_param[1]);
                    foreach (string i in get_param[1].Split('&'))
                    {
                        string[] k_v = i.Split('=');
                        if (k_v.Length > 1)
                            RequestParameters.Add(k_v[0], k_v[1]);
                    }
                }
                RequestHeader.Add("url",get_param[0]);
            }
            else
            {
                RequestHeader.Add("url", t[1]);
            }
            RequestHeader.Add("version", t[2]);
            for (int i=1;i<reqs.Length;i++)
            {
                t = reqs[i].Trim('\r',' ').Split(':');
                RequestHeader.Add(t[0], t[1]);
            }
            if (t[1].Length > 0)
            {
                string request_body = datas[1].Trim(' ', '\r', '\n');
                request_body = System.Web.HttpUtility.UrlDecode(request_body);
                string[] paras = request_body.Split('&');
                if (paras.Length > 0)
                {
                    for (int i = 0; i < paras.Length; i++)
                    {
                        string[] k_v = paras[i].Trim(' ', '\r', '\n').Split('=');
                        if (k_v.Length > 1)
                            RequestParameters.Add(k_v[0], k_v[1]);
                    }
                }
            }
        }
    }
}
