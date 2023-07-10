using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Tools.JsonSerializer
{
    class Serializer {
        public String getObject(Object obj) {
            String result = "{";
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach(PropertyInfo info in properties) {
                result += "\"" + info.Name + "\":"+ ConvertObjToStr(obj,info)+",";
            }
            result.Remove(result.Length - 1);
            result += "}";
            return result;
        }
        public String getArray(Array arr) {
            String result = "[";
            Type eletype = arr.GetType().GetElementType();
            for(int i = 0;i < arr.Length;i++) {
                result += ConvertArrToStr(arr,i) + ",";
            }
            result.Remove(result.Length - 1);
            result += "]";
            return result;
        }
        public String ConvertArrToStr(Array arr,int index) {
            String res = "";
            object obj = arr.GetValue(index);
            if (obj.GetType() == typeof(int)
                || obj.GetType() == typeof(double)
                || obj.GetType() == typeof(float)) {
                res = "" + obj;
            } else if (obj.GetType() == typeof(string)) {
                res = "\"" + obj + "\"";
            } else {
                res = getObject(obj);
            }
            return res;
        }
        public String ConvertObjToStr(Object obj,PropertyInfo p) {
            if (p == null)
                throw new Exception("找不到对应的属性");
            String res = "";
            if (p.PropertyType == typeof(int)
                || p.PropertyType == typeof(double)
                || p.PropertyType == typeof(float)) {
                res = "" + p.GetValue(obj);
            } else if (p.PropertyType == typeof(string)) {
                res = "\"" + p.GetValue(obj) + "\"";
            } else if (p.PropertyType == null) {
                res = "null";
            } else {
                if (p.PropertyType.IsArray) {
                    res = getArray(p.GetValue(obj) as Array);
                } else {
                    res = getObject(p.GetValue(obj));
                }
            }
            return res;
        }
    }
}
