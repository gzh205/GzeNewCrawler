using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server
{
    public class Session
    {
        /// <summary>
        /// 服务器中所有的session列表
        /// </summary>
        public static Dictionary<string, Session> sessions = new Dictionary<string, Session>();
        /// <summary>
        /// session的唯一标识
        /// </summary>
        public string sessionid { get; private set; }
        /// <summary>
        /// Session的有效期，如果当前时间超过有效期，则判定session无效
        /// </summary>
        public DateTime efftime { get; private set; }
        /// <summary>
        /// 一个会话的所有全局变量
        /// </summary>
        public Dictionary<string, object> global_variables = new Dictionary<string, object>();
        /// <summary>
        /// 返回这个session创建的时间
        /// </summary>
        /// <returns></returns>
        public DateTime createTime()
        {
            string[] sss = sessionid.Split('|');
            if (sss.Length != 4)
                throw new Exceptions.InvalidSessionException("这个session有问题:" + sessionid);
            string[] date_arr = sss[1].Trim().Split('/');
            string[] time_arr = sss[2].Trim().Split(new char[] { ':', ' ' });
            return new DateTime(Convert.ToInt32(date_arr[2]), Convert.ToInt32(date_arr[0]), Convert.ToInt32(date_arr[1]),
                Convert.ToInt32(time_arr[0]), Convert.ToInt32(time_arr[1]), Convert.ToInt32(time_arr[2]));
        }
        public static Session CreateSession()
        {
            Session result = new Session();
            //一个session应该由随机数+当前时间+进程id组成
            DateTime now = DateTime.Now;
            //sessionid=随机数|M/d/yyyy|h:mm:ss tt|pid
            result.sessionid = Tools.RandomStringGenerator.Generate(Convert.ToInt32(Config.Cfg.config_strings["session_length"])) + "|" + now.ToShortDateString() +
                "|" + now.ToLongTimeString() + "|" + System.Diagnostics.Process.GetCurrentProcess().Id;
            result.efftime = now.AddSeconds(Convert.ToDouble(Config.Cfg.config_strings["session_timeout"]));
            return result;
        }
        /// <summary>
        /// 根据session字符串获取或者创建一个新的session，一般是因为浏览器长时间为与服务器交互导致服务器删除了session
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Session CreateSession(string sessionID)
        {
            sessions.ContainsKey(sessionID);
            Session result = null;
            try
            {
                result = Session.sessions[sessionID];
            }
            catch (KeyNotFoundException)
            {
                result = new Session();
            }
            result.sessionid = sessionID;
            //重新将有效时间设为当前时间+session_timeout的秒数
            result.efftime = DateTime.Now.AddSeconds(Convert.ToDouble(Config.Cfg.config_strings["session_timeout"]));
            Session.sessions[result.sessionid] = result;
            return result;
        }
        /// <summary>
        /// 清理长期没使用的会话
        /// </summary>
        public static void ClearSession()
        {
            foreach(var sess in sessions.ToArray())
            {
                if (sess.Value.efftime < DateTime.Now)
                {
                    sessions.Remove(sess.Key);
                }
            }
        }
        /// <summary>
        /// 更新会话
        /// </summary>
        /// <param name="sessionID"></param>
        public static void UpdateSession(string sessionID)
        {
            sessions[sessionID].efftime.AddSeconds(Convert.ToDouble(Config.Cfg.config_strings["session_timeout"]));
        }
    }
}
