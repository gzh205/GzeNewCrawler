using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using BlogWebsite.cdpf.task;

namespace BlogWebsite.cdpf
{
    /// <summary>
    /// 
    /// </summary>
    public class Sql
    {
        private static Sql inst;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Sql getSql()
        {
            if (inst == null)
                inst = new Sql();
            return inst;
        }
        private Sql()
        {
        }
        //下面是多线程处理逻辑
        private Queue<Tasks> tasks = new Queue<Tasks>();
        private Semaphore tasksNum;
        private Semaphore queuereader;
        private int threadnum;
        private Thread[] threads;
        private bool isrunning;
        public Semaphore callThread = new Semaphore(0, 1);
        void insertQueue(Tasks task)
        {
            tasks.Enqueue(task);
            tasksNum.Release();
            callThread.WaitOne();
        }
        private void start()
        {
            isrunning = true;
            threads = new Thread[threadnum];
            for (int i = 0; i < threadnum; i++)
            {
                threads[i] = new Thread(threadproc);
                threads[i].Start();
            }
        }
        /// <summary>
        /// 停止持久化框架的运行
        /// </summary>
        public void finish()
        {
            isrunning = false;
        }
        private void threadproc()
        {
            while (isrunning)
            {
                //获取一个任务
                queuereader.WaitOne();
                tasksNum.WaitOne();
                Tasks task = tasks.Dequeue();
                queuereader.Release();
                //处理任务
                task.run(task.table);
                //完成处理
                callThread.Release();
            }
        }
        /// <summary>
        /// 进行初始化设置，并自动开始运行框架
        /// </summary>
        /// <param name="filename">配置XML文件名</param>
        /// 其中xml文件可以参考DBSettings.xml
        /// filename指用于数据库连接的程序集名称
        public void init(string filename)
        {
            Config config = Config.LoadConfig(filename);
            //获取所有的连接字符串并逐一添加
            foreach (add n in config.connectionNodes)
            {
                Uri u = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), n.filename);
                try
                {
                    Connection.addConnection(n.Namespace, n.connectionstring, Assembly.LoadFrom(u.ToString()).GetType(n.classname + ".dll"));
                }
                catch (System.IO.FileNotFoundException)
                {
                    u = new Uri(new Uri(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()), n.filename + ".dll");
                    Connection.addConnection(n.Namespace, n.connectionstring, Assembly.LoadFrom(u.ToString()).GetType(n.classname));
                }
            }
            threadnum = config.threadNum;
            tasksNum = new Semaphore(0, config.threadNum);
            queuereader = new Semaphore(1, 1);
            if (config.cachesize < 1) throw new SqlException("缓存的最大元素个数不得小于1");
            Cache.Size = config.cachesize;
            start();
        }
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<T> selectsome<T>(string str)
        {
            Connection con = new Connection();
            return con.SelectSome<T>(str).ToList();
        }
        /// <summary>
        /// 查询obj中[PrimaryKey]所对应的数据
        /// </summary>
        /// <param name="obj">只需要设置[PrimaryKey]，其他属性或字段会自动填充</param>
        public void select(Table obj)
        {
            insertQueue(new Select(obj));
        }
        /// <summary>
        /// 查询obj中[PrimaryKey]所对应的数据
        /// </summary>
        /// <param name="obj">只需要设置[PrimaryKey]，其他属性或字段会自动填充</param>
        public void insert(Table obj)
        {
            insertQueue(new Insert(obj));
        }
        /// <summary>
        /// 查询obj中[PrimaryKey]所对应的数据
        /// </summary>
        /// <param name="obj">只需要设置[PrimaryKey]，其他属性或字段会自动填充</param>
        public void update(Table obj)
        {
            insertQueue(new Update(obj));
        }
        /// <summary>
        /// 查询obj中[PrimaryKey]所对应的数据
        /// </summary>
        /// <param name="obj">只需要设置[PrimaryKey]，其他属性或字段会自动填充</param>
        public void delete(Table obj)
        {
            insertQueue(new Delete(obj));
        }
    }
}
