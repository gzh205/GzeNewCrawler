using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Exceptions
{
    public class InvalidNameException:Exception
    {
        public InvalidNameException(string message) : base(message)
        {
        }
    }
}
