using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/show_blog")]
    public class ShowBlog : Page
    {
        public override Response get(Request request)
        {
            List<models.Blog_View> blogs = cdpf.Sql.getSql().selectsome<models.Blog_View>("");
            Tools.JsonEncoder json = new Tools.JsonEncoder().CreateArray<models.Blog_View>(blogs, "blogs");
            return new Server.response_impl.StringData(json.GetData());
        }

        public override Response post(Request request)
        {
            return Response.Empty;
        }
    }
}
