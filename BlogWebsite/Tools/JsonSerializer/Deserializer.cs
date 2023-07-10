using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Tools.JsonSerializer {
    class Deserializer {
        private Dictionary<string, PropertyInfo> property_type = new Dictionary<string, PropertyInfo>();
        public void getArray(Object obj,String jsonstr) {
            int leftBrace = 0;
            int leftBracket = 0;
            int index = 0;
            Type element = obj.GetType().GetElementType();
            PropertyInfo[] fields = element.GetProperties();
            foreach(PropertyInfo f in fields)
            {
                JsonProperty attr_ = f.GetCustomAttribute(typeof(Tools.JsonProperty)) as JsonProperty;
                if (attr_ == null)
                {
                    property_type.Add(f.Name, f);
                }
                else
                {
                    property_type.Add(attr_.Name, f);
                }
            }
            var modelList = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { obj.GetType().GetElementType() }));
            var addMethod = modelList.GetType().GetMethod("Add");
            var ToArray = modelList.GetType().GetMethod("ToArray");
            for (int i = 0;i < jsonstr.Length;i++) {
                switch (jsonstr[i]) {
                    case ',': {
                            if (leftBrace == 0 && leftBracket == 1) {
                                setArray(obj,addMethod,element,jsonstr.Substring(index,i - index).Trim(' ','\n','\r','\t'));
                                index = i;
                            }
                            break;
                        }
                    case '{': {
                            leftBrace++;
                            break;
                        }
                    case '}': {
                            leftBrace--;
                            break;
                        }
                    case '[': {
                            leftBracket++;
                            break;
                        }
                    case ']': {
                            if (leftBrace == 0 && leftBracket == 1) {
                                setArray(obj,addMethod,element,jsonstr.Substring(index,i - index).Trim(' ','\n','\r','\t'));
                                index = i;
                            }
                            leftBracket--;
                            break;
                        }
                }
            }
            obj = addMethod.Invoke(obj,null);
        }
        public void getObject(Object obj,String jsonstr) {
            int leftBrace = 0;
            int leftBracket = 0;
            int index = 0;
            string name = "";
            for (int i = 0;i < jsonstr.Length;i++) {
                switch (jsonstr[i]) {
                    case ':': {
                            if (leftBrace == 1 && leftBracket == 0) {
                                name = jsonstr.Substring(index,i - index).Trim(' ','\t','\n','\r','"');
                                index = i + 1;
                            }
                            break;
                        }
                    case ',': {
                            if (leftBrace == 1 && leftBracket == 0) {
                                PropertyInfo p_ = property_type[name];
                                setProperty(obj, p_, jsonstr.Substring(index,i - index).Trim(' ','\n','\r'));
                            }
                            break;
                        }
                    case '{': {
                            index = i + 1;
                            leftBrace++;
                            break;
                        }
                    case '}': {
                            if(leftBrace == 1 && leftBracket == 0) {
                                PropertyInfo p_ = property_type[name];
                                setProperty(obj, p_, jsonstr.Substring(index,i - index).Trim(' ','\n','\r'));
                            }
                            leftBrace--;
                            break;
                        }
                    case '[': {
                            index = i + 1;
                            leftBracket++;
                            break;
                        }
                    case ']': {
                            leftBracket--;
                            break;
                        }
                }
            }
        }
        private void setArray(Object obj,MethodInfo add,Type element,String intmpstr) {
            if(element == typeof(string)) {
                add.Invoke(obj,new object[] { intmpstr.Trim('"') });
            }else if(element == typeof(double)) {
                add.Invoke(obj,new object[] { Convert.ToDouble(intmpstr) });
            }else if (element == typeof(float)) {
                add.Invoke(obj,new object[] { Convert.ToSingle(intmpstr) });
            }else if (element == typeof(int)){
                add.Invoke(obj,new object[] { Convert.ToInt32(intmpstr) });
            } else {
                object tmp = Activator.CreateInstance(element);
                getObject(tmp,intmpstr);
                add.Invoke(obj,new object[] { tmp });
            }
        }
        private void setProperty(Object obj,PropertyInfo p,String intmpstr) {
            if (p == null)
                throw new Exception("找不到对应的属性");
            if (p.PropertyType == typeof(int)) {
                p.SetValue(obj,Convert.ToInt32(intmpstr));
            } else if (p.PropertyType == typeof(double)) {
                p.SetValue(obj,Convert.ToDouble(intmpstr));
            } else if (p.PropertyType == typeof(float)) {
                p.SetValue(obj,Convert.ToSingle(intmpstr));
            } else if (p.PropertyType == typeof(string)) {
                p.SetValue(obj,intmpstr.Trim('"'));
            } else {
                Object tmp;
                if (p.PropertyType.IsArray) {
                    tmp = Array.CreateInstance(p.PropertyType.GetElementType(),0);
                    getArray(tmp,intmpstr);
                } else {
                    tmp = Activator.CreateInstance(p.PropertyType);
                    getObject(tmp,intmpstr);
                }
                p.SetValue(obj,tmp);
            }
        }
    }
}