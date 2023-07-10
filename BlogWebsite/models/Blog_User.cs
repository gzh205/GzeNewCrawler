using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.cdpf;
namespace BlogWebsite.models
{
    class Blog_User:Table
    {
        [PrimaryKey]
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
    }
}
