using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/create_blog")]
    class CreateBlog : Page
    {
        public override Response get(Request request)
        {
            return new Server.response_impl.WebPageData("/webpages/create_blog.html");
        }

        public override Response post(Request request)
        {
            return Response.Empty;
        }
    }
}
