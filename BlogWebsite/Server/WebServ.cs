using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using BlogWebsite.ServerScript;
using System.Text;

namespace BlogWebsite.Server
{
    public class WebServ
    {
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
            if (param == null)
            {
                return;
            }
            Socket client = param as Socket;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                byte[] arr = new byte[client.ReceiveBufferSize];
                client.Receive(arr);
                if(arr.Length != client.ReceiveBufferSize)
                {
                    break;
                }
                sb.Append(System.Text.Encoding.UTF8.GetString(arr));
            }
            string data = sb.ToString();
            Console.WriteLine("当前是否正在连接:" + client.Connected);
            Console.WriteLine(data);
            //分析HTTP请求
            Request request = null;
            try
            {
                request = new Request(data);
            }
            catch (Exceptions.InvalidRequestException e)
            {
                Console.WriteLine(e.Message);
                client.Close();
                return;
            }
            
            if (request.RequestHeader["url"] == "/")
            {
                //如果请求的URL为"/"，则说明浏览器想打开主页
                request.RequestHeader["url"] += Config.Cfg.config_strings["default_page"];
            }
            //使用Page子类定义的POST和GET函数处理请求
            Page page = null;
            try
            {
                page = Page.GetPage(request.RequestHeader["url"]);
                page.call(request);
            }
            catch (KeyNotFoundException)
            {
                //Page.GetPage找不到想要的请求url对应的处理类，说明浏览器想请求文件
                try
                {
                    string url = string.Empty;
                    if (!request.RequestHeader.TryGetValue("url", out url))
                        throw new Exceptions.InvalidNameException("url 错误");
                    if (!url.Contains("."))
                        throw new Exceptions.InvalidNameException("url 错误");
                    page = FileResponse.staticcall(request);
                }
                catch (Exception exc)
                {
                    //找不到指定的文件，所以回复404
                    SendHeader("http/1.1", "text/html", 0, "404 Not Found", client);
                    SendToBrowser("cannot find request file", client);
                    Console.WriteLine("error,url:" + request.RequestHeader["url"]);
                    return;
                }
            }
            string conn = "";
            //keep-alive处理
            if (request.RequestHeader.TryGetValue("Connection",out conn) && conn=="keep-alive")
            {
                page.other_data += "Connection: keep-alive\r\n";
            }
            else
            {
                page.other_data += "Connection: close\r\n";
            }
            //不对响应文本进行压缩，因为这会影响性能，而且不便于实现
            page.other_data += "Content-Encoding:null\r\n";
            SendHeader("http/1.1", page.ContentType, page.ResponseData.Length, page.StatusCode, client, page.other_data);
            SendToBrowser(page.ResponseData, client);
            if (conn != "keep-alive")//如果浏览器没有指名要keep-alive，则关闭连接
            {
                client.Close();
            }
        }
        public void SendHeader(string sHttpVersion, string sMIMEHeader, int iTotBytes, string sStatusCode, Socket mySocket, string otherData = "")
        {
            String sBuffer = "";
            if (sMIMEHeader==null || sMIMEHeader.Length == 0)
            {
                sMIMEHeader = "text/html"; // 默认 text/html
            }
            //http协议版本号+状态码
            sBuffer = sBuffer + sHttpVersion + " " + sStatusCode + "\r\n";
            sBuffer = sBuffer + "Allow: GET,POST";
            //服务器名称
            sBuffer = sBuffer + "Server: " + Config.Cfg.config_strings["server_name"] + "\r\n";
            //响应的语言
            sBuffer = sBuffer + "Content-Language: en,zh\r\n";
            //响应的编码，可以通过gzip对文件进行压缩提高数据传输的速度，这里暂时不实现。
            //sBuffer = sBuffer + "Content-Encoding: utf-8";
            //文件类型
            sBuffer = sBuffer + "Content-Type: " + " " + sMIMEHeader + "\r\n";
            //接受字节数据
            sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
            //文件的长度
            sBuffer = sBuffer + "Content-Length: " + " " + iTotBytes + "\r\n";
            //其他的信息
            if (otherData != "")
            {
                sBuffer += otherData;
            }
            sBuffer += "\r\n";
            Byte[] bSendData = System.Text.Encoding.UTF8.GetBytes(sBuffer);
            SendToBrowser(bSendData, mySocket);
            Console.WriteLine("Total Bytes : " + iTotBytes.ToString());
        }
        public void SendToBrowser(String sData, Socket mySocket)
        {
            SendToBrowser(System.Text.Encoding.UTF8.GetBytes(sData), mySocket);
        }

        public void SendToBrowser(Byte[] bSendData, Socket mySocket)
        {
            int numBytes = 0;
            try
            {
                if (mySocket.Connected)
                {
                    if ((numBytes = mySocket.Send(bSendData, bSendData.Length, 0)) == -1)
                        Console.WriteLine("Socket Error cannot Send Packet");
                    else
                    {
                        Console.WriteLine("No. of bytes send {0}", numBytes);
                    }
                }
                else
                    Console.WriteLine("连接失败....");
            }
            catch (Exception e)
            {
                Console.WriteLine("发生错误 : {0} ", e);
            }
        }

    }
}