using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server.response_impl
{
    class StringData:Response
    {
        /// <summary>
        /// 想要返回给浏览器的字符串数据（可以使json文本）
        /// </summary>
        /// <param name="data"></param>
        public StringData(string data)
        {
            this.data = data;
            this.content_type = "html";
        }

        public override string data { get; protected set; }
        public override string content_type { get; protected set; }

        public override byte[] run(Session session)
        {
            return Response.encoding.GetBytes(data);
        }
    }
}
