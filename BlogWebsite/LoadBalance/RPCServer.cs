using BlogWebsite.DynamicCompiler.Core;
using BlogWebsite.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsite.LoadBalance
{
    internal class RPCServer
    {
        private Dictionary<string, List<RPCClientInfo>> rpc_dict = new Dictionary<string, List<RPCClientInfo>>();
        private Dictionary<int, CallHash> rpc_call = new Dictionary<int, CallHash>();
        private List<RPCClientInfo> clients;
        private TcpListener listener;//tcp监听
        private bool isStop;//是否停止服务器
        public void listenProc()
        {
            //启动成功
            while (!isStop)
            {
                try
                {
                    // 接收连接请求
                    Socket tcpClient = listener.AcceptSocket();
                    //建立Ftp连接
                    Thread t = new Thread(new ParameterizedThreadStart(RequestProcessing));
                    t.IsBackground = true;
                    t.Start(tcpClient);
                }
                catch
                {
                    break;
                }
            }
        }
        public void Start()
        {
            listener = new TcpListener(IPAddress.Parse("0.0.0.0"), Convert.ToInt32(Config.Cfg.config_strings["port"]));
            listener.Start();
            isStop = false;
            Thread t = new Thread(listenProc);
            t.Start();
        }

        public void Stop()
        {
            listener.Stop();
            isStop = true;
        }

        public void RequestProcessing(object param)
        {
            Socket client = param as Socket;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                byte[] arr = new byte[client.ReceiveBufferSize];
                client.Receive(arr);
                if (arr.Length != client.ReceiveBufferSize)
                {
                    break;
                }
                sb.Append(System.Text.Encoding.UTF8.GetString(arr));
            }
            //接收负载均衡器的输入
            try
            {
                JsonRPCData data = Tools.JsonSerializer.Json.DeSerialize<JsonRPCData>(sb.ToString()) as JsonRPCData;
                if (data.jsonrpc == "register" && data.method == "register")
                {
                    List<string> methods = data.param;
                    foreach (string m in methods)
                    {
                        try
                        {
                            rpc_dict[m].Add(new RPCClientInfo(client, methods));
                        }
                        catch (NullReferenceException e)
                        {
                            rpc_dict[m] = new List<RPCClientInfo>
                        {
                            new RPCClientInfo(client, methods)
                        };
                        }
                    }
                }
                else
                {
                    string idx = client.RemoteEndPoint.ToString() + data.method + RandomStringGenerator.Generate(10);
                    int hash_code = idx.GetHashCode();
                    data.id = hash_code;
                    RPCClientInfo info = rpc_dict[data.method][0];
                    foreach (RPCClientInfo i in rpc_dict[data.method])
                    {
                        if (info.Count < i.Count)
                        {
                            info = i;
                        }
                    }
                    info.Acquire();
                    CallHash call_ = new CallHash();
                    call_.old_idx = data.id;
                    call_.new_hash = hash_code;
                    call_.method = data.method;
                    call_.info = info;
                    rpc_call[hash_code] = call_;
                    data.id = hash_code;
                    string send_dat = Tools.JsonSerializer.Json.Serializer(data);
                    client.Send(System.Text.Encoding.Default.GetBytes(send_dat));
                }
            }catch(Exception e)
            {
                JsonRPCResult res = Tools.JsonSerializer.Json.DeSerialize<JsonRPCResult>(sb.ToString()) as JsonRPCResult;
                CallHash call_ = rpc_call[res.id];
                res.id = call_.old_idx;
                call_.info.Release();
                string send_dat = Tools.JsonSerializer.Json.Serializer(res);
                call_.info.socket.Send(System.Text.Encoding.Default.GetBytes(send_dat));
            }
        }
    }
}
