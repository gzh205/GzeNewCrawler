using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Server;
using BlogWebsite.cdpf;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/login")]
    public class Login : Page
    {
        public override Response get(Request request)
        {
            return Response.Empty;
        }

        public override Response post(Request request)
        {
            //检查验证码
            try
            {
                string kaptcha = "" + (int)session.global_variables["yzm"];
                if(kaptcha == request.RequestParameters["kaptcha"])
                {
                    session.global_variables["yzm_error"] = "hidden";
                    //读取数据库
                    models.Blog_User usr = new models.Blog_User();
                    usr.email = request.RequestParameters["email"];
                    Sql.getSql().select(usr);
                    if(usr.password == request.RequestParameters["pwd"])
                    {
                        session.global_variables["pwd_error"] = "hidden";
                        session.global_variables["user_info"] = usr;
                        return new Server.response_impl.WebPageData("/webpages/blogpage.html");
                    }
                    else
                    {
                        session.global_variables["pwd_error"] = "visible";
                    }
                }
                else
                {
                    session.global_variables["yzm_error"] = "visible";
                }
                return new Server.response_impl.WebPageData("/index.html");
            }
            catch (KeyNotFoundException)
            {
                session.global_variables["yzm_error"] = "visible";
            }
            return new Server.response_impl.WebPageData("/index.html");
        }
    }
}
