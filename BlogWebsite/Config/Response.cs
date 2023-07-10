using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Config
{
    public class Response
    {
        public static Dictionary<int, string> code = new Dictionary<int, string>();
        public static void Load()
        {
            StreamReader sr = new StreamReader(new FileStream("ResponseCode.csv", FileMode.Open));
            string[] datas = sr.ReadToEnd().Split('\n');
            foreach(string s in datas)
            {
                string[] d = s.Split(',');
                if (d.Length != 2)
                {
                    continue;
                }
                code.Add(Convert.ToInt32(d[0].Trim(' ', '\r', '\t')), d[1].Trim(' ', '\r', '\t'));
            }
        }
    }
}
