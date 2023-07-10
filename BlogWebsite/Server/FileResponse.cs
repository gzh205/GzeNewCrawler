using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server
{
    public class FileResponse:Page
    {
        /// <summary>
        /// 只有请求文件时才会调用此方法
        /// </summary>
        public static FileResponse staticcall(Request request)
        {
            FileResponse page = new FileResponse();
            //处理浏览器发过来的请求
            //首先确定浏览器有没有发cookies，cookies主要保存用户信息和sessionID
            try
            {
                page.setCookies(request.RequestHeader["Cookie"]);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                //如果触发这个异常，说明没有cookie
            }
            //创建或者更新session
            page.session = page.setSession();
            try
            {
                page.Cookies["sessionid"] = page.session.sessionid;
            }
            catch (KeyNotFoundException)
            {
                page.Cookies.Add("sessionid", page.session.sessionid);
            }
            //根据不同的GET和POST请求，调用各自的方法，返回一个Response对象
            page.session = page.setSession();
            page.other_data = "";
            if (request.RequestHeader["method"].ToUpper() == "GET")
            {
                FileLoader fl = FileLoader.GetFileB(request.RequestHeader["url"]);
                page.ContentType = fl.content_type;
                page.StatusCode = "200 OK";
                page.ResponseData = fl.databin;
            }
            else
            {
                page.StatusCode = "405 Method Not Allowed";
                //这里应该返回一个接受的请求列表，但是我不知道应该怎么写
            }
            page.setCookies();
            return page;
        }
        /// <summary>
        /// 不要调用它
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override Response get(Request request)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 不要调用它
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override Response post(Request request)
        {
            throw new NotImplementedException();
        }
    }
}
