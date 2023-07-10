using BlogWebsite.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.LoadBalance
{
    internal class JsonRPCData
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        [JsonProperty(Name = "params")]
        public List<string> param { get; set; }
        public int id { get; set; }
    }
}
