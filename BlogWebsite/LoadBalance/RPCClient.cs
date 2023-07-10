using BlogWebsite.DynamicCompiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsite.LoadBalance
{
    internal class RPCClient
    {
        private static Dictionary<string, RPC.RPC> rpc_dict = new Dictionary<string, RPC.RPC>();
        private bool isStop;//是否停止服务器
        private Socket server_sock = new Socket(SocketType.Stream,ProtocolType.Tcp);
        private string server_ip;
        private int server_port;
        public RPCClient(string ip,int port)
        {
            server_ip = ip;
            server_port = port;
        }
        public void Reg()
        {
            List<string> procedures = new List<string>();
            //自动搜索RPC并注册
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in types)
            {
                if (t.FullName.Contains("BlogWebsite.RPC") && t.BaseType == typeof(RPC.RPC))
                {
                    procedures.Add(t.Name);
                    rpc_dict.Add(t.Name, Activator.CreateInstance(t) as RPC.RPC);
                }
            }
            server_sock.Connect(server_ip, server_port);
            Thread thread = new Thread(ThreadProc);
            thread.Start();
        }
        public void ThreadProc()
        {
            //启动成功
            while (!isStop)
            {
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    byte[] arr = new byte[server_sock.ReceiveBufferSize];
                    server_sock.Receive(arr);
                    if (arr.Length != server_sock.ReceiveBufferSize)
                    {
                        break;
                    }
                    sb.Append(System.Text.Encoding.UTF8.GetString(arr));
                }
                string res = CallRPC(sb.ToString());
                server_sock.Send(System.Text.Encoding.Default.GetBytes(res));
            }
        }
        private string CallRPC(string json)
        {
            JsonRPCData data = Tools.JsonSerializer.Json.DeSerialize<JsonRPCData>(json) as JsonRPCData;
            string error = null;
            string result = null;
            try
            {
                result = rpc_dict[data.method].Run(data.param);
            }
            catch(Exception e)
            {
                error = e.Message;
            }
            JsonRPCResult json_res = new JsonRPCResult();
            json_res.result = result;
            json_res.error = error;
            json_res.id = data.id;
            return Tools.JsonSerializer.Json.Serializer(json_res);
        }
    }
}
