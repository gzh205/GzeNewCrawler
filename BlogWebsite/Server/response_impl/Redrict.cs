using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server.response_impl
{
    class Redrict : Response
    {
        public string url { get; private set; }
        public override string data { get; protected set; }
        public override string content_type { get; protected set; }

        public override byte[] run(Session session)
        {
            return new byte[0];
        }

        public Redrict(string url)
        {
            this.url = url;
            data = "";
        }
    }
}
