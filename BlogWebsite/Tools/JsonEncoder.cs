using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Tools
{
    public class JsonEncoder
    {
        private string json_data;
        public JsonEncoder()
        {
            json_data = "";
        }
        public string GetData()
        {
            return "{" + json_data + "}";
        }
        public JsonEncoder CreateObject(Object obj, string name)
        {
            json_data += "\"" + name + "\":{" + obj + "}";
            return this;
        }
        public JsonEncoder CreateArray<T>(List<T> array, string name)
        {
            CreateArray(array.ToArray(), name);
            return this;
        }
        public JsonEncoder CreateArray<T>(T[] array, string name)
        {
            json_data += "\"" + name + "\":[";
            foreach (T t in array)
            {
                json_data += t + ",";
            }
            json_data = json_data.Substring(0, json_data.Length - 1);
            json_data += "]";
            return this;
        }
    }
}
