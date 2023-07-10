using BlogWebsite.cdpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.models
{
    public class Blog_Info:Table
    {
        [PrimaryKey]
        public string blog_id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime submit_date { get; set; }
        public string email { get; set; }
        public string comment { get; set; }
        public string img { get; set; }
    }
}
