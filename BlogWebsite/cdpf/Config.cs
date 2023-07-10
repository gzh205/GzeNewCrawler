using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml;
using System.IO;

namespace BlogWebsite.cdpf
{
    /// <summary>
    /// 用于加载配置文件的类，无需使用者调用
    /// </summary>
    internal class add
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public add() { }
        public add(string Namespace, string filename, string classname, string connectionstring)
        {
            this.classname = classname;
            this.connectionstring = connectionstring;
            this.filename = filename;
            this.Namespace = Namespace;
        }
        /// <summary>
        /// DbConnecton子类的类名
        /// </summary>
        public string classname { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string connectionstring { get; set; }
        /// <summary>
        /// 程序集文件名
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 数据库连接所对应的命名空间
        /// </summary>
        public string Namespace { get; set; }
    }
    /// <summary>
    /// 用于加载配置文件的类，无需使用者调用
    /// </summary>
    internal class Config
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Config() { connectionNodes = new List<add>(); }
        /// <summary>
        /// 包含有类名和连接字符串的xml节点
        /// </summary>
        public List<add> connectionNodes { get; set; }
        public int threadNum { get; set; }
        public int cachesize { get; set; }
        public static Config LoadConfig(string xmlFile)
        {
            Config c = new Config();
            StreamReader sr = new StreamReader(new FileStream(xmlFile, FileMode.Open));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();
            sr.Dispose();
            XmlNodeList nodes = doc.SelectNodes("Config/add");
            foreach (XmlNode n in nodes)
            {
                c.connectionNodes.Add(new add(n.Attributes["namespace"].Value, n.Attributes["filename"].Value, n.Attributes["classname"].Value, n.Attributes["connectionstring"].Value));
            }
            c.threadNum = Convert.ToInt32(doc.SelectSingleNode("Config/threadnum").InnerText);
            c.cachesize = Convert.ToInt32(doc.SelectSingleNode("Config/cachesize").InnerText);
            return c;
        }
    }
}
