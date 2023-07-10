using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf.task
{
    class Select : Tasks
    {
        public Select(object obj) : base(obj)
        {

        }
        public override object run(object obj)
        {
            string dat = obj.GetType().Namespace;
            string name = obj.GetType().Name;
            object result = Cache.getInstance().getData(dat, name, obj);
            if (result == null)
            {
                Connection c = new Connection();
                c.Select(obj);
                result = obj;
            }
            return CpoyObject(result);
        }
    }
}
