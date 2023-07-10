using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf
{
    public class Table
    {
        public override string ToString()
        {
            string result = "{";
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();
            foreach(PropertyInfo p in properties)
            {
                result += "\"" + p.Name + "\":\"" + p.GetValue(this) + "\",";
            }
            foreach(FieldInfo f in fields)
            {
                result += "\"" + f.Name + "\":\"" + f.GetValue(this) + "\",";
            }
            return result.Substring(0, result.Length - 1) + "}";
        }
    }
}
