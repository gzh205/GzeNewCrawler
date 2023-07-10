using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Security.Cryptography;
using BlogWebsite.DynamicCompiler.Core;

namespace BlogWebsite.MySQL
{
    internal class MySQLConnector : DbConnection
    {
        private Dictionary<string, string> param = new Dictionary<string, string>();
        private List<string> keys = new List<string>();
        private string para;
        private Socket client_socket;
        private string server_ip;
        private int server_port;
        private int pwd_insert_idx;
        private string auth_plugin_name;
        private string conn_data;
        public MySQLConnector(string ip, int port, string username, string password, string client_name)
        {
            //先进行预处理
            //添加连接参数
            this.param.Add("_os", "Windows");
            keys.Add("_os");
            this.param.Add("_client_name", client_name);
            keys.Add("_client_name");
            this.param.Add("_pid", Convert.ToString(System.Diagnostics.Process.GetCurrentProcess().Id));
            keys.Add("_pid");
            this.param.Add("_thread", Convert.ToString(Thread.CurrentThread.ManagedThreadId));
            keys.Add("_thread");
            this.param.Add("_platform", "AMD64");
            keys.Add("_platform");
            this.param.Add("_client_version", "3.2.3");
            keys.Add("_client_version");
            this.param.Add("_server_host", ip);
            keys.Add("_server_host");
            StringBuilder para_sb = new StringBuilder();
            foreach (string _k in this.keys)
            {
                string v = this.param[_k];
                char _l_k = (char)_k.Length;
                char _l_v = (char)v.Length;
                para_sb.Append(_l_k + _k);
                para_sb.Append(_l_v + v);
            }
            this.para = para_sb.ToString();
            //计算密码的SHA1
            this.PreEncrpy(password);
            //初始化socket
            this.client_socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server_ip = ip;
            server_port = port;
            //数据库登录
            StringBuilder sb = new StringBuilder();
            string capabilities = new string(new char[]{ (char)0x85, (char)0xa6, (char)0xff, (char)0x80 });
            sb.Append(capabilities);
            string MAX_Packet = new string(new char[]{ (char)0, (char)0, (char)0, (char)0x40 });
            sb.Append(MAX_Packet);
            string character_set = new string(new char[]{ (char)0x21 });//编码方式
            sb.Append(character_set);
            char[] char_t = new char[23];
            for(int i=0;i<23; i++)
            {
                char_t[i] = (char)0;
            }
            string filler = new string(char_t);
            sb.Append(filler);
            //string username 用户名
            string _end_flag = new string(new char[]{ (char)0 });
            sb.Append(username + _end_flag);
            this.pwd_insert_idx = 38 + username.Length;
            string auth_response_length = new string(new char[] { (char)20 });//因为密码是SHA1加密后的密文，而SHA1长度固定为20
            sb.Append(auth_response_length);
            char_t = new char[20];
            for (int i = 0; i < 20; i++)
            {
                char_t[i] = (char)0;
            }
            string pwd = new string(char_t);
            sb.Append(pwd);
            sb.Append(this.auth_plugin_name);
            sb.Append(new string(new char[] { (char)0 }));
            string _para_len = new string( new char[]{ (char)(this.para.Length) });
            sb.Append(_para_len);
            sb.Append(this.para);
            string _raw_data = sb.ToString();
            this.conn_data = CreateMySqlPackage(_raw_data)[0];
            char[] t_ = conn_data.ToCharArray();
            t_[3] = (char)1;
            conn_data = new string(t_);
        }
        private List<string> CreateMySqlPackage(string data)
        {
            //每个数据包的大小最大为2^24字节
            int max_length = 0x1000000 - 1;
            List<string> packages = new List<string>();
            string tmp = data;
            string _id = new string(new char[]{ (char)0 });
            int _data_len = data.Length;
            while (true)
            {
                if (_data_len < max_length)
                {
                    string _len = BlogWebsite.Tools.Utils.MyInt2String(tmp.Length);
                    packages.Add(_len + _id + tmp);
                    break;
                }
                else
                {
                    char _FF = (char)0xFF;
                    string _header = new string(new char[]{ _FF, _FF, _FF, (char)0 });
                    string _t = tmp.Substring(0, max_length);
                    tmp = tmp.Substring(max_length, tmp.Length - max_length);
                    packages.Add(_header + _t);
                }
            }
            return packages;
        }
        private string pwd1;
        private string pwd2;
        private void PreEncrpy(string pwd)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            SHA1 sha1 = SHA1.Create();
            this.pwd1 = encoder.GetString(sha1.ComputeHash(Encoding.ASCII.GetBytes(pwd)));
            this.pwd2 = encoder.GetString(sha1.ComputeHash(Encoding.ASCII.GetBytes(pwd1)));
        }

        public override string ConnectionString
        {
            get
            {
                return ConnectionString;
            }
            set
            {
                ConnectionString = value;
            }
        }

        public override string Database
        {
            get
            {
                return Database;
            }
        }

        public override string DataSource
        {
            get
            {
                return DataSource;
            }
        }

        public override string ServerVersion
        {
            get
            {
                return ServerVersion;
            }
        }

        public override ConnectionState State
        {
            get
            {
                return State;
            }
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand()
        {
            return new MySQLCommand();
        }
    }
}
