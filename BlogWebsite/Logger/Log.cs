using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Logger
{
    public class Log
    {
        private static StreamWriter writer;
        /// <summary>
        /// 记录服务器的信息，并保存到文件中（主要用于调试阶段）
        /// </summary>
        /// <param name="str"></param>
        public static void info(string str)
        {
            if (writer == null)
                writer = new StreamWriter(new FileStream(Config.Cfg.config_strings["log_file_name"],FileMode.OpenOrCreate));
            writer.WriteLine(str);
        }
        public static void Dispose()
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
        }
    }
}
