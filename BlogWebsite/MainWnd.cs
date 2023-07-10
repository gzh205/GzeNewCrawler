using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlogWebsite.Server;

namespace BlogWebsite
{
    public partial class MainWnd : Form
    {
        private Server.WebServ server;
        public MainWnd()
        {
            InitializeComponent();
        }
        public static bool isStart = false;
        private void MainWnd_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(MainWnd_FormClosing);//注册窗体关闭事件
            DynamicCompiler.CompilerCore.loadSystemApi();
            //反射自动调用Load方法
            Type[] types = Assembly.GetEntryAssembly().GetTypes();
            //添加WebPages下的网页对象
            foreach (Type t in types)
            {
                if (t.BaseType == typeof(Server.Page))
                {            
                    try
                    {
                        Attribute attr = t.GetCustomAttribute(typeof(PageInfo));
                        PageInfo info = attr as PageInfo;
                        string key = info.Name;
                        if (key[0] != '/')
                            key = "/" + key;
                        Server.Page.pages.Add(key, t);
                    }
                    catch (Exception)
                    {
                        Server.Page.pages.Add(t.Name, t);
                    }               
                }
                switch (t.Namespace)
                {
                    case "BlogWebsite.Config":
                        {
                            //调用Config下所有类的静态方法Load()
                            MethodInfo mi = t.GetMethod("Load");
                            mi.Invoke(null, new object[] { });
                        }
                        break;
                }
            }
            //将输入输出流绑定到文本框中
            new TextIO.TextConsoleReader(this.richTextBox1);
            new TextIO.TextConsoleWriter(this.richTextBox1);
            //创建服务器
            server = new WebServ();
            //显示端口号
            this.textBox1.Text = Config.Cfg.config_strings["port"];
            //启动session清理器
            this.session_clear.Interval = Convert.ToInt32(Config.Cfg.config_strings["session_timeout"]) * 1000;
            this.session_clear.Start();
            //初始化数据持久化框架
            cdpf.Sql.getSql().init("DBSettings.xml");
            //初始化邮件服务
            Tools.MailService.init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isStart)
            {
                isStart = true;
                Config.Cfg.config_strings["port"] = textBox1.Text.Trim();
                server.Start();
                Console.WriteLine("服务器已启动");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isStart)
            {
                isStart = false;
                server.Stop();
                Console.WriteLine("服务器已停止");
            }
        }

        private void session_clear_Tick(object sender, EventArgs e)
        {
            //循环定时清理session
            Session.ClearSession();
        }

        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                button2_Click(sender, null);//停止服务器
                e.Cancel = true;
                Application.Exit();//关闭程序
            }
        }
    }
}
