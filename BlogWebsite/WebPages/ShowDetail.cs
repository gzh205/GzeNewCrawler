using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/show_detail")]
    public class ShowDetail : Page
    {
        public override Response get(Request request)
        {
            models.Blog_Info blog = new models.Blog_Info();
            blog.blog_id = request.RequestParameters["blog_id"];
            cdpf.Sql.getSql().select(blog);
            models.Blog_User usr = new models.Blog_User();
            usr.email = blog.email;
            cdpf.Sql.getSql().select(usr);
            session.global_variables["title"] = blog.title;
            session.global_variables["name"] = usr.name;
            session.global_variables["content"] = blog.content;
            session.global_variables["submit_date"] = blog.submit_date;
            session.global_variables["comment"] = blog.comment;
            session.global_variables["img"] = blog.img;
            session.global_variables["blog_id"] = blog.blog_id;
            return new Server.response_impl.WebPageData("/webpages/blog_detail.html");
        }

        public override Response post(Request request)
        {
            return Server.Response.Empty;
        }
    }
}
