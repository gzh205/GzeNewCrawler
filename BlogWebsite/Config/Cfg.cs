using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Config
{
    public class Cfg
    {
        public static Dictionary<string, string> config_strings = new Dictionary<string, string>();
        public static Dictionary<string, string> content_type = new Dictionary<string, string>();
        public static void Load()
        {
            StreamReader sr = new StreamReader(new FileStream("config.txt", FileMode.Open));
            string[] settings = sr.ReadToEnd().Split('\n');
            sr.Close();
            sr.Dispose();
            foreach(string s in settings)
            {
                if (s.Length == 0)
                    continue;
                int end_str = s.IndexOf('#');
                string ds = s;
                if (end_str != -1)
                    ds = s.Substring(0,end_str);
                string[] tmp = ds.Split('=');
                if (tmp.Length != 2)
                {
                    continue;
                }
                config_strings.Add(tmp[0].Trim(' ', '\r', '\t'), tmp[1].Trim(' ', '\r', '\t'));
            }
            //加载FileContent.csv
            sr = new StreamReader(new FileStream("FileContent.csv", FileMode.Open));
            settings = sr.ReadToEnd().Split('\n');
            foreach(string s in settings)
            {
                if (s.Length == 0)
                    continue;
                string[] k_v = s.Split(',');
                if (k_v.Length != 2)
                    continue;
                string key = k_v[0].Trim(' ', '\r', '\t', '.');
                if(!content_type.ContainsKey(key))
                    content_type.Add(key, k_v[1].Trim(' ', '\r','\t'));
            }
            sr.Close();
            sr.Dispose();
        }
    }
}
