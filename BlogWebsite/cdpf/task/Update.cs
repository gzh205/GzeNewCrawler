using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf.task
{
    class Update : Tasks
    {
        public Update(object obj) : base(obj)
        {
        }
        public override object run(object obj)
        {
            Connection c = new Connection();
            bool r = c.Update(obj);
            if (r)
            {
                if (r)
                {
                    string dat = obj.GetType().Namespace;
                    string name = obj.GetType().Name;
                    Cache.getInstance().addData(dat, name, obj);
                }
            }
            return r;
        }
    }
}
