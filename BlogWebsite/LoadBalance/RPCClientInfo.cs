using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsite.LoadBalance
{
    public struct CallHash
    {
        public int new_hash;
        public int old_idx;
        public string method;
        public RPCClientInfo info;
    }
    public class RPCClientInfo
    {
        public RPCClientInfo() { }
        public RPCClientInfo(Socket socket,List<string> methods)
        {
            this.socket = socket;
            this.methods = methods;
        }
        public Socket socket;
        public List<string> methods;
        public int Count = 0;
        Mutex mutex = new Mutex(false, "MutexForCount");
        public void Acquire()
        {
            mutex.WaitOne();
            Count++;
            mutex.ReleaseMutex();
        }
        public void Release()
        {
            mutex.WaitOne();
            Count--;
            mutex.ReleaseMutex();
        }
    }
    
}