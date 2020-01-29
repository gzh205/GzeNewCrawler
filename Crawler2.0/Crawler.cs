using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Crawler2
{
    public abstract class Crawler
    {
        public List<Uri> Urls;
        public Queue queue { get; private set; }
        private Thread[] threads{ set; get; }
        public int threadNum;
        private Semaphore read;
        public int depth;
        private Semaphore queueNum;
        public static int RetryTime;
        public Crawler()
        {
            queue = new Queue();
            this.threadNum = 1;    
            threads = new Thread[threadNum];
            this.read = new Semaphore(1,1);
            this.depth = 1;
            this.Urls = new List<Uri>();
            this.queueNum = new Semaphore(0, int.MaxValue);
        }
        public Crawler setThreadNum(int num)
        {
            if (threadNum > 0)
            {
                this.threadNum = num;
            }
            return this;
        }
        public Crawler setRetryTime(int num)
        {
            RetryTime = num;
            return this;
        }
        public Crawler setDepth(int depth)
        {
            this.depth = depth;
            return this;
        }
        public void run(string url)
        {
            Uri uri = new Uri(url);
            this.Urls.Add(uri);
            this.queue.Enqueue(Core.getCore(uri,1));
            this.queueNum.Release();
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(ThreadProc);
                threads[i].Start();
            }
        }
        public void ThreadProc()
        {
            while (true) 
            {
                read.WaitOne();
                queueNum.WaitOne();
                Core core = (Core)queue.Dequeue();
                read.Release();
                core.getUrls();
                this.pageProcesser(core);
                if (core.depth < depth)
                {
                    if (core.urlLists == null) continue;
                    foreach (Uri u in core.urlLists)
                    {
                        if (!(Urls.Contains(u) || u.AbsoluteUri.Contains("javascript:") || u.AbsoluteUri == "#"))
                        {
                            Core c = Core.getCore(u, core.depth + 1);
                            if (c == null) continue;
                            Urls.Add(u);
                            queue.Enqueue(c);
                            queueNum.Release();
                        }
                        else Console.WriteLine(u.AbsoluteUri + "--droped");
                    }
                }
                else if (queue.Count == 0)
                    break;
            }
        }
        public abstract void pageProcesser(Core core);
    }
}
