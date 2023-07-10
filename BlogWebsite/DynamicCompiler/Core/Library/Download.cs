using System.Net;

namespace BlogWebsite.DynamicCompiler.Core.Library
{
    public class Download : Function
    {
        public Download()
        {
            parametername.Add("input");
        }
        public override Data run()
        {
            Data res = parametervalue[0];
            if (!res.isEmpty())
            {
                if (!res.isStr)
                {
                    throw new SyntaxErrorException("Download函数的参数必须为字符串");
                }
                WebClient wc = new WebClient();
                res.str = wc.DownloadString(res.str);
            }
            return res;
        }
    }
}
