using BlogWebsite.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogWebsite.LoadBalance
{
    internal class JsonRPCResult
    {
        public string result { get; set; }
        public string error { get; set; }
        public int id { get; set; }
    }
}
