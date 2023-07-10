using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Tools
{
    public class MailService
    {
        public static string host;
        public static string userName;
        public static string password;
        public static void init()
        {
            host = Config.Cfg.config_strings["smtp_host"];
            userName = Config.Cfg.config_strings["smtp_userName"];
            password = Config.Cfg.config_strings["smtp_password"];
        }
        public static void Send(string addr,string url)
        {
            string url_in_email = "http://" + Config.Cfg.config_strings["ip"] + ":" + Config.Cfg.config_strings["port"] + url;
            SendMailUse(addr, "博客系统", "您刚才在博客系统注册了新账户，请点击下面的链接完成验证<br/>" + url_in_email);

        }
        private static void SendMailUse(string addr,string title,string data)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
            client.Host = host;//邮件服务器
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(userName, password);//用户名、密码
            //////////////////////////////////////
            string strfrom = userName;
            string strto = addr;
            string subject = title;//邮件的主题
            string body = data;//发送的邮件正文
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(strfrom, "xyf");
            msg.To.Add(strto);
            msg.Subject = subject;//邮件标题   
            msg.Body = body;//邮件内容   
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = true;//是否是HTML邮件   
            msg.Priority = MailPriority.High;//邮件优先级   
            try
            {
                client.Send(msg);
                Console.WriteLine("发送成功");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine(ex.Message, "发送邮件出错");
            }
        }
    }
}
