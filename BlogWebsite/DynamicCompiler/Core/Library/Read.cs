using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DynamicCompiler.Core.Library
{
    public class Read : Function
    {
        public Read()
        {
        }
        public override Data run()
        {
            string res = Console.ReadLine();
            return new Data(res);
        }
    }
}
