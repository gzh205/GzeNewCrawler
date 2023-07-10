using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DynamicCompiler.Core.Library
{
    public class Get : Function
    {
        public Get()
        {
            parametername.Add("input");
        }

        public override Data run()
        {
            Data res = parametervalue[0];
            if (res.isStr)
            {
                return new Data("GET:" + res.str);
            }
            else
            {
                return new Data("这一处语法有错误");
            }
        }
    }
}
