using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/commit_comment")]
    public class CommitComment : Page
    {
        public override Response get(Request request)
        {
            return Server.Response.Empty;
        }

        public override Response post(Request request)
        {
            string blog_id = session.global_variables["blog_id"] as string;
            string txt = request.RequestParameters["txt_comment"];
            models.Blog_Info info = new models.Blog_Info();
            info.blog_id = blog_id;
            cdpf.Sql.getSql().select(info);
            info.comment += "<tr><td>" + txt + "</td></tr>";
            cdpf.Sql.getSql().update(info);
            session.global_variables["comment"] += info.comment;
            return new Server.response_impl.WebPageData("/webpages/blog_detail.html");
        }
    }
}
