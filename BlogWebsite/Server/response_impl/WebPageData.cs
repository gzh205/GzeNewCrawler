using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server.response_impl
{
    class WebPageData : Response
    {
        /// <summary>
        /// 想要返回给浏览器的网页名
        /// </summary>
        /// <param name="filename">服务器会自动根据文件名打开网页并执行网页中的服务器脚本</param>
        public WebPageData(string filename)
        {
            FileLoader fl = FileLoader.GetFile(filename);
            data = fl.data;
            content_type = fl.content_type;
        }

        public override string data { get; protected set; }
        public override string content_type { get; protected set; }

        public override byte[] run(Session session)
        {
            try
            {
                return Response.encoding.GetBytes(ServerScript.ScriptController.eval(session, data));
            }
            catch(Exception e)
            {
                return Response.encoding.GetBytes("服务器脚本语法错误:"+e.Message);
            }
        }
    }
}
