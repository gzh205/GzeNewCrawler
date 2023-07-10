using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/blogpages")]
    public class BlogPage : Page
    {
        public override Response get(Request request)
        {
            return new Server.response_impl.WebPageData("/webpages/blogpage.html");
        }

        public override Response post(Request request)
        {
            return Response.Empty;
        }
    }
}
