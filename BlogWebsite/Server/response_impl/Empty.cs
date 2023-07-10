using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server.response_impl
{
    class Empty : Response
    {
        public override string data { get; protected set; }
        public override string content_type { get; protected set; }

        public override byte[] run(Session session)
        {
            content_type = "text/html";
            data = "empty";
            return Response.encoding.GetBytes(data);
        }
    }
}
