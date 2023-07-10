using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server
{
    public class FileLoader
    {
        //文件的数据
        public byte[] databin { get; private set; }
        public string data { get; private set; }
        //文件长度
        //文件类型
        public string content_type { get; private set; }
        /// <summary>
        /// 二进制形式读取文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static FileLoader GetFileB(string filename)
        {
            FileLoader result = new FileLoader();
            //搜索对应资源
            string root_dir = Config.Cfg.config_strings["root_dir"];
            //分析问文件类型
            string[] splited_name = filename.Split('.');
            if (splited_name.Length <= 1)
            {
                throw new Exceptions.InvalidNameException("文件名:" + filename + "不合法");
            }
            result.content_type = GetContent(splited_name[splited_name.Length - 1]);
            string full_filename = root_dir + filename;
            FileStream fs = new FileStream(full_filename, FileMode.Open);
            result.databin = new byte[fs.Length];
            fs.Read(result.databin, 0, Convert.ToInt32(fs.Length));
            return result;
        }

        /// <summary>
        /// 字符串形式读取文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static FileLoader GetFile(string filename)
        {
            FileLoader result = new FileLoader();
            //搜索对应资源
            string root_dir = Config.Cfg.config_strings["root_dir"];
            //分析问文件类型
            string[] splited_name = filename.Split('.');
            if (splited_name.Length <= 1)
            {
                throw new Exceptions.InvalidNameException("文件名:" + filename + "错误");
            }
            result.content_type = GetContent(splited_name[splited_name.Length - 1]);
            if (filename[0] == '/')
            {
                filename = "." + filename;
            }
            else if(filename[0] != '.')
            {
                filename = "./" + filename;
            }
            string full_filename = new Uri(new Uri(root_dir + "\\"),filename).LocalPath;
            try
            {
                StreamReader sr = new StreamReader(new FileStream(full_filename, FileMode.Open));
                result.data = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("file" + full_filename + "not found");
            }
            return result;
        }
        /// <summary>
        /// 获取文件格式(详见FileContent.csv)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetContent(string name)
        {
            string result = string.Empty;
            try
            {
                result = Config.Cfg.content_type[name.ToLower()];
            }
            catch (KeyNotFoundException)
            {
                result = Config.Cfg.content_type["default"];
            }
            return result;
        }
    }
}
