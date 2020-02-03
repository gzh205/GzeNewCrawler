using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler2
{
    public class Mirror : Crawler
    {
        public override void pageProcesser(Core core)
        {
            FileStream fs = new FileStream("D:\\test\\web\\" + main.num + ".html", FileMode.Create);
            main.num++;
            StreamWriter sw = new StreamWriter(fs,core.webPage.Encoding);
            sw.Write(core.webPage.Text);
            fs.Close();
        }
    }
}
