using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BlogWebsite.Tools
{
    public class JsonProperty: Attribute
    {
        public string Name { get; set; }
    }
}
