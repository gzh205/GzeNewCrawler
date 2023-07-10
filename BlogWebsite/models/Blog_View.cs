using BlogWebsite.cdpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.models
{
    public class Blog_View:Table
    {
        [PrimaryKey]
        public string blog_id { get; set; }
        public string title { get; set; }
        public DateTime submit_date { get; set; }
    }
}
