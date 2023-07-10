using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Exceptions;

namespace BlogWebsite.Server.response_impl
{
    class BinaryData:Response
    {
        public BinaryData(string filename)
        {
            data = filename;
            FileLoader fl = FileLoader.GetFileB(data);
            content_type = fl.content_type;
            res = fl.databin;
        }
        public BinaryData(byte[] data)
        {
            this.data = "";
            res = data;
        }
        public override string data { get; protected set; }
        public override string content_type { get; protected set; }
        public byte[] res { get; protected set; }

        public override byte[] run(Session session)
        {
            return res;
        }
    }
}
