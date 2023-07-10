using BlogWebsite.cdpf;
using BlogWebsite.models;
using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    public class ValidCell
    {
        public ValidCell(string email, string user_name, string password)
        {
            this.email = email;
            this.password = password;
            this.user_name = user_name;
        }
        public string email { get; private set; }
        public string random_str { get; private set; }
        public string user_name { get; private set; }
        public string password { get; private set; }
    }
    [PageInfo(Name = "/valid_reg")]
    public class ValidReg : Page
    {
        public static Dictionary<string, ValidCell> reg_info = new Dictionary<string, ValidCell>();
        public override Response get(Request request)
        {
            string valid_random_str = request.RequestParameters["val"];
            ValidCell cell = null;
            if (reg_info.TryGetValue(valid_random_str, out cell))
            {
                session.global_variables["valid_result"] = "您已成功注册账号";
                Blog_User usr = new Blog_User();
                usr.email = cell.email;
                usr.name = cell.user_name;
                usr.password = cell.password;
                Sql.getSql().insert(usr);
            }
            else
            {
                session.global_variables["valid_result"] = "邮箱验证失败";
            }
            return new Server.response_impl.WebPageData("/webpages/valid_email.html");
        }

        public override Response post(Request request)
        {
            return new Server.response_impl.Empty();
        }
    }
}
