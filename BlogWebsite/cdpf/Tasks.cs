using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Reflection;

namespace BlogWebsite.cdpf
{
    internal abstract class Tasks
    {
        private Tasks()
        {
        }
        public Tasks(object table)
        {
            this.table = table;
        }
        public object table;
        public abstract object run(object obj);
        public static object CpoyObject(object o)
        {
            Type t = o.GetType();
            object result = Activator.CreateInstance(t);
            PropertyInfo[] properties = t.GetProperties();
            FieldInfo[] fields = t.GetFields();
            foreach (PropertyInfo i in properties)
            {
                i.SetValue(result, i.GetValue(o));
            }
            foreach (FieldInfo i in fields)
            {
                i.SetValue(result, i.GetValue(o));
            }
            return result;
        }
    }
}