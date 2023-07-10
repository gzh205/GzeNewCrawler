using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Exceptions
{
    class UnsupportedMethod:Exception
    {
        public UnsupportedMethod(string message) : base(message) { }
    }
}
