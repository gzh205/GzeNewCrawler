using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler2
{
    class main
    {
        static void Main()
        {
            new Mirror().setDepth(5).setRetryTime(0).setThreadNum(8).run("https://baike.baidu.com/item/%E7%BD%91%E7%BB%9C%E7%88%AC%E8%99%AB/5162711?fromtitle=%E7%88%AC%E8%99%AB&fromid=22046949&fr=aladdin");
        }
        public static int num = 0;
    }
}
