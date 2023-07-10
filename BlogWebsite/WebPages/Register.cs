using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/reg")]
    public class Register : Page
    {
        public override Response get(Request request)
        {
            return new Server.response_impl.WebPageData("register.html");
        }

        public override Response post(Request request)
        {
            //获取register.html发送给后台的参数
            string email = request.RequestParameters["email"];
            string pwd = request.RequestParameters["pwd"];
            string name = request.RequestParameters["name"];
            //收集参数，并生成指定的url
            string paras = Tools.RandomStringGenerator.Generate(Convert.ToInt32(Config.Cfg.config_strings["valid_url_length"]));
            //根据url发送邮件
            Tools.MailService.Send(email, "/valid_reg?val="+paras);
            ValidReg.reg_info[paras] = new ValidCell(email, name, pwd);
            return new Server.response_impl.Redrict("/webpages/waiting_reg.html");
        }
    }
}
