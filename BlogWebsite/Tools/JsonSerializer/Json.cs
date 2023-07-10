using System;
using System.Reflection;
namespace BlogWebsite.Tools.JsonSerializer
{
    public class Json {
        public static Object DeSerialize<T>(String Json) {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            PropertyInfo[] properties = type.GetProperties();
            Deserializer de = new Deserializer();
            de.getObject(t,Json);
            return t;
        }
        public static String Serializer(Object obj) {
            Serializer se = new Serializer();
            return se.getObject(obj);
        }
    }
}
