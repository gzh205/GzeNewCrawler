using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public SqlException(string msg) : base(msg)
        {

        }
    }
}
