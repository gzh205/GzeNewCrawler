using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Server;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/index")]
    public class Index : Page
    {
        public override Response get(Request request)
        {
            session.global_variables["pwd_error"] = "hidden";
            session.global_variables["yzm_error"] = "hidden";
            return new Server.response_impl.WebPageData("/index.html");
        }

        public override Response post(Request request)
        {
            return Response.Empty;
        }
    }
}
