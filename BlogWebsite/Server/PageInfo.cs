using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Server
{
    public class PageInfo:Attribute
    {
        public string Name { get; set; }
    }
}
