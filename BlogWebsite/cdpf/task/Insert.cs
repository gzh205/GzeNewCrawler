using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf.task
{
    class Insert : Tasks
    {
        public Insert(object obj) : base(obj)
        {
        }
        public override object run(object obj)
        {
            Connection c = new Connection();
            bool r = c.Insert(obj);
            if (r)
            {
                string dat = obj.GetType().Namespace;
                string name = obj.GetType().Name;
                Cache.getInstance().addData(dat, name, obj);
            }
            return r;
        }
    }
}
