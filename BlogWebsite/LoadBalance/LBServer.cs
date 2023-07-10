using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsite.LoadBalance
{
    internal class LBServer
    {
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
            //接收负载均衡器的输入

        }
    }
}
