using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server
{
    public abstract class Response
    {
        /// <summary>
        /// 如果你希望服务器在处理完请求后什么数据也不返回，就在get或post函数中return Response.Empty
        /// </summary>
        public static Response Empty = new response_impl.Empty();
        /// <summary>
        /// 设置服务器响应的编码方式，默认为UTF-8
        /// </summary>
        public static Encoding encoding = Encoding.UTF8;
        /// <summary>
        /// 将数据进一步处理，以便返回给浏览器
        /// </summary>
        /// <param name="session">用户的会话</param>
        /// <returns></returns>
        public abstract byte[] run(Session session);
        public abstract string data { get; protected set; }
        public abstract string content_type { get; protected set; }
    }
}