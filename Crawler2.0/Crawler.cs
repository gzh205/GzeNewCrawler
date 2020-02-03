using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Crawler2
{
    /// <summary>
    /// Crawler需要实现抽象方法PageProcessor才能使用
    /// </summary>
    public abstract class Crawler
    {
        /// <summary>
        /// 已经打开过页面的url列表
        /// </summary>
        public List<Uri> Urls;
        /// <summary>
        /// 网页队列，用于生产者/消费者模式的线程协调
        /// </summary>
        public Queue queue { get; private set; }
        /// <summary>
        /// 线程数组
        /// </summary>
        private Thread[] threads{ set; get; }
        /// <summary>
        /// 线程数量
        /// </summary>
        public int threadNum;
        /// <summary>
        /// 读写信号量，用于保证同一时间只能有一个线程访问网页队列
        /// </summary>
        private Semaphore read;
        /// <summary>
        /// 爬虫的爬取深度
        /// </summary>
        public int depth;
        /// <summary>
        /// 用于标记网页队列中元素的个数，当队列中没有元素时，读取操作缩在的线程会被阻塞
        /// </summary>
        private Semaphore queueNum;
        /// <summary>
        /// 当页面打开失败时，重试的次数
        /// </summary>
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
        /// <summary>
        /// 调用run方法开始运行爬虫
        /// </summary>
        /// <param name="url"></param>
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
        /// <summary>
        /// 线程函数
        /// </summary>
        public void ThreadProc()
        {
            while (true) 
            {
                read.WaitOne();
                queueNum.WaitOne();
                Core core = (Core)queue.Dequeue();
                read.Release();
                if (core.connect())
                {
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
        }
        public abstract void pageProcesser(Core core);
    }
}
