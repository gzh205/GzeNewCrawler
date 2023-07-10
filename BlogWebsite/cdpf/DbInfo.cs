using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf
{
    internal class DbInfo
    {
        public DbInfo(string connectionString, Type sqlConn)
        {
            this.sqlConn = sqlConn;
            this.connectionString = connectionString;
        }
        public string connectionString { get; private set; }
        public Type sqlConn { get; private set; }
    }
}
