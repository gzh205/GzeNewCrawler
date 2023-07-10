using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/upload_blog")]
    public class UploadBlog : Page
    {
        public override Response get(Request request)
        {
            return Response.Empty;
        }

        public override Response post(Request request)
        {
            models.Blog_User user = session.global_variables["user_info"] as models.Blog_User;
            if (user == null)
            {
                return new Server.response_impl.StringData("user_error");
            }
            else
            {
                models.Blog_Info blog = new models.Blog_Info();
                blog.blog_id = DateTime.Now.ToString() + "__" + Tools.RandomStringGenerator.Generate(10);
                blog.email = user.email;
                blog.content = request.RequestParameters["txt_comment"];
                blog.comment = "";
                blog.img = request.RequestParameters["upload_img"];
                blog.title = request.RequestParameters["title"];
                cdpf.Sql.getSql().insert(blog);
                return new Server.response_impl.Redrict("/show_detail?blog_id=" + blog.blog_id);
            }
        }
    }
}
