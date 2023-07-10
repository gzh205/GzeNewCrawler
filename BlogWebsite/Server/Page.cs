using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server
{
    [PageInfo(Name="")]
    public abstract class Page
    {
        public static Dictionary<string, Type> pages = new Dictionary<string, Type>();
        public Dictionary<string, string> Cookies = new Dictionary<string, string>();
        /// <summary>
        /// 返回一个Page的子类
        /// 如果没有请求url所对应的Page类，则抛出KeyNotFoundException
        /// </summary>
        /// <param name="request">请求的url</param>
        /// <returns></returns>
        public static Page GetPage(string request)
        {
            return Activator.CreateInstance(Page.pages[request]) as Page;
        }
        /// <summary>
        /// 服务器返回数据的二进制形式
        /// </summary>
        public byte[] ResponseData { get; set; }
        /// <summary>
        /// 服务器状态码
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 服务器返回的数据类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// web重定向的url
        /// </summary>
        public string other_data { get; set; }
        public Session session { get; set; }
        /// <summary>
        /// 该函数用于处理Cookies
        /// </summary>
        /// <param name="cookieStr"></param>
        protected void setCookies(string cookieStr)
        {
            string[] cookie_strs = cookieStr.Split(';');
            foreach(string coo in cookie_strs)
            {
                string[] t = coo.Split('=');
                if (t.Length != 2)
                    continue;
                Cookies.Add(t[0].Trim(), t[1].Trim());
            }
        }
        /// <summary>
        /// 设置session
        /// </summary>
        protected Session setSession()
        {
            //读取session
            try
            {
                string session = Cookies["sessionid"];
                return Session.CreateSession(session);
            }
            catch (KeyNotFoundException)
            {
                return Session.CreateSession();
            }
        }
        /// <summary>
        /// 该函数用于处理HTTP请求的请求头，并调用GET和POST方法，由WebServ自动调用
        /// </summary>
        /// <param name="request"></param>
        public void call(Request request)
        {
            //处理浏览器发过来的请求
            //首先确定浏览器有没有发cookies，cookies主要保存用户信息和sessionID
            try
            {
                setCookies(request.RequestHeader["Cookie"]);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                //如果触发这个异常，说明没有cookie
            }
            //创建或者更新session
            session = setSession();
            try
            {
                Cookies["sessionid"] = session.sessionid;
            }
            catch (KeyNotFoundException)
            {
                Cookies.Add("sessionid", session.sessionid);
            }
            //将额外响应头设为空串
            other_data = "";
            //根据不同的GET和POST请求，调用各自的方法，返回一个Response对象
            Response response = Response.Empty;
            switch (request.RequestHeader["method"].ToUpper())
            {
                case "GET":
                    response = get(request);
                    break;
                case "POST":
                    response = post(request);
                    break;
                default:
                    StatusCode = "500 Internal Server Error";
                    throw new Exceptions.UnsupportedMethod("不支持"+request.RequestHeader["method"]+"请求");
            }
            if (response == null)
                response = Response.Empty;
            ResponseData = response.run(session);
            ContentType = response.content_type;
            if (response is response_impl.Redrict)
            {
                //如果设置了重定向链接，则将返回码设为307
                StatusCode = "302 Temporary Redirect";
                other_data += "Location:" + (response as response_impl.Redrict).url + "\r\n";
            }
            else
            {
                StatusCode = "200 OK";
            }
            setCookies();
        }
        /// <summary>
        /// 设置响应字符串
        /// </summary>
        /// <param name="data"></param>
        public void addResponseStr(string data)
        {
            other_data += data;
        }
        /// <summary>
        /// 设置Set-Cookies的响应头
        /// </summary>
        /// <returns></returns>
        protected void setCookies()
        {
            other_data += "Set-Cookie: ";
            foreach (string k in Cookies.Keys)
            {
                other_data +=  k + "=" + Cookies[k] + "; path=/";
            }
            other_data += "\r\n";
        }
        public abstract Response get(Request request);
        public abstract Response post(Request request);
        public void setGlobalVariable(string key, object value)
        {
            if (session.global_variables.ContainsKey(key))
            {
                session.global_variables[key] = value;
            }
            else
            {
                session.global_variables.Add(key, value);
            }
        }
        public object getGlobalVariable(string key)
        {
            return session.global_variables[key];
        }

    }
}