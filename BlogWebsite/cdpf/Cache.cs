using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf
{
    /// <summary>
    /// 数据库缓存，使用LRU算法管理数据，通过字典高效搜索数据。
    /// </summary>
    public class Cache
    {
        //单例模式
        private static Cache inst;
        public static int Size;
        public static Cache getInstance()
        {
            if (inst == null)
                inst = new Cache();
            return inst;
        }
        private Cache()
        {
        }
        /// <summary>
        /// 数据库缓存
        /// </summary>
        public LRUList<object> datas = new LRUList<object>();
        public Dictionary<string, List<int>> mapping = new Dictionary<string, List<int>>();
        public List<StringBuilder> DBnames = new List<StringBuilder>();
        public List<StringBuilder> TableNames = new List<StringBuilder>();
        /// <summary>
        /// 获取一个索引表
        /// </summary>
        /// <param name="dbname">数据库名</param>
        /// <param name="tablename">表名</param>
        /// <returns>索引表</returns>
        private List<int> getMapping(string dbname, string tablename)
        {
            string searchstr = dbname + "." + tablename;
            List<int> indexs;
            try
            {
                indexs = mapping[searchstr];
                //先尝试在缓存中寻找需要的数据
            }
            catch (KeyNotFoundException)
            {//索引表为空则新建一个表
                indexs = new List<int>();
                mapping.Add(searchstr, indexs);
            }
            return indexs;
        }
        /// <summary>
        /// 调整一条数据的位置
        /// </summary>
        private void changeIndex(List<int> indexs, int n, object data)
        {
            datas.Remove(data);
            datas.AddLast(data);
            indexs.Remove(n);
            indexs.Add(datas.Count - 1);
        }
        /// <summary>
        /// 尝试在缓存中寻找数据，若没有找到，则返回null
        /// </summary>
        /// <param name="dbname">数据库名</param>
        /// <param name="tablename">表名</param>
        /// <param name="obj">数据的对象</param>
        /// <returns>返回查询的结果，若没找到，则为空</returns>
        public object getData(string dbname, string tablename, object obj)
        {
            object result = null;
            List<int> indexs = getMapping(dbname, tablename);
            //先获取obj的主键
            object pk = null;
            PropertyInfo[] propertiesobj = obj.GetType().GetProperties();
            foreach (PropertyInfo p in propertiesobj)
            {
                if (p.IsDefined(typeof(PrimaryKey)))
                {
                    pk = p.GetValue(obj);
                    break;
                }
            }
            if (pk == null)
            {
                FieldInfo[] fieldsobj = obj.GetType().GetFields();
                foreach (FieldInfo p in fieldsobj)
                {
                    if (p.IsDefined(typeof(PrimaryKey)))
                    {
                        pk = p.GetValue(obj);
                        break;
                    }
                }
            }
            if (pk == null)
                throw new SqlException("表中没有主键或主键上没有添加[PrimaryKey]属性");
            if (indexs.Count != 0)
            {//索引表的长度不为0时，搜索缓存，否则直接返回null
                int headindex = indexs[0];
                PropertyInfo[] properties = datas[headindex].GetType().GetProperties();
                PropertyInfo resproperty = null;
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].IsDefined(typeof(PrimaryKey)))
                    {
                        resproperty = properties[i];
                        break;
                    }
                }
                if (resproperty == null)
                {
                    FieldInfo[] fields = datas[headindex].GetType().GetFields();
                    FieldInfo resfield = null;
                    for (int j = 0; j < fields.Length; j++)
                    {
                        if (fields[j].IsDefined(typeof(PrimaryKey)))
                        {
                            resfield = fields[j];
                            break;
                        }
                    }
                    if (resfield == null)
                        return null;
                    else
                    {
                        for (int j = 0; j < indexs.Count; j++)
                        {
                            object tmp = datas[indexs[j]];
                            if (resfield.GetValue(tmp) == pk)
                            {
                                result = tmp;
                                changeIndex(indexs, j, tmp);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < indexs.Count; j++)
                    {
                        object tmp = datas[indexs[j]];
                        if (resproperty.GetValue(tmp) == pk)
                        {
                            result = tmp;
                            changeIndex(indexs, j, tmp);
                            break;
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="dbname">数据库名</param>
        /// <param name="tablename">表名</param>
        /// <param name="value"></param>
        public void addData(string dbname, string tablename, object value)
        {
            List<int> indexs = getMapping(dbname, tablename);
            if (datas.Count >= Size)
            {
                //删除数据以及对应索引
                object tmp = datas[0];
                string dat = tmp.GetType().Namespace;
                List<int> tmpindex = getMapping(dat, tmp.GetType().Name);
                if (tmpindex.Count != 0)
                {//索引表的长度不为0时，搜索缓存，否则直接返回null
                    for (int i = 0; i < tmpindex.Count; i++)
                    {
                        if (equals(tmp, datas[tmpindex[i]]))
                        {
                            tmpindex.RemoveAt(i);
                            break;
                        }
                    }
                }
                datas.Remove(tmp);
            }
            indexs.Add(datas.Count);
            datas.AddLast(value);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="dbname">数据库名</param>
        /// <param name="tablename">表名</param>
        /// <param name="value"></param>
        public void removeData(string dbname, string tablename, object value)
        {
            List<int> indexs = getMapping(dbname, tablename);
            if (indexs.Count != 0)
            {//索引表的长度不为0时，搜索缓存，否则直接返回null
                int headindex = indexs[0];
                PropertyInfo[] properties = datas[headindex].GetType().GetProperties();
                PropertyInfo resproperty = null;
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].IsDefined(typeof(PrimaryKey)))
                    {
                        resproperty = properties[i];
                        break;
                    }
                }
                if (resproperty == null)
                {
                    FieldInfo[] fields = datas[headindex].GetType().GetFields();
                    FieldInfo resfield = null;
                    for (int j = 0; j < fields.Length; j++)
                    {
                        if (fields[j].IsDefined(typeof(PrimaryKey)))
                        {
                            resfield = fields[j];
                            break;
                        }
                    }
                    if (resfield == null)
                        return;
                    else
                    {
                        for (int j = 0; j < indexs.Count; j++)
                        {
                            object tmp = datas[indexs[j]];
                            if (resfield.GetValue(tmp) == resfield.GetValue(value))
                            {
                                datas.Remove(tmp);
                                indexs.Remove(j);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < indexs.Count; j++)
                    {
                        object tmp = datas[indexs[j]];
                        if (resproperty.GetValue(tmp) == resproperty.GetValue(value))
                        {
                            datas.Remove(tmp);
                            indexs.Remove(j);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 比较两对象是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool equals(object a, object b)
        {
            PropertyInfo[] aproperties = a.GetType().GetProperties();
            FieldInfo[] afields = a.GetType().GetFields();
            PropertyInfo[] bproperties = b.GetType().GetProperties();
            FieldInfo[] bfields = b.GetType().GetFields();
            if (aproperties.Length == bproperties.Length && afields.Length == bfields.Length)
            {
                for (int i = 0; i < aproperties.Length; i++)
                {
                    if (aproperties[i] != bproperties[i])
                        return false;
                }
                for (int i = 0; i < afields.Length; i++)
                {
                    if (afields[i] != bfields[i])
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}