using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Exceptions
{
    public class SingletonConstructError:Exception
    {
        public SingletonConstructError(string message) : base(message)
        {
        }
    }
}
