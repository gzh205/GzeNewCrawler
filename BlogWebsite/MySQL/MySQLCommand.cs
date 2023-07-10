using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.MySQL
{
    internal class MySQLCommand : DbCommand
    {
        public MySQLCommand()
        {

        }
        public override string CommandText
        {
            get
            {
                return CommandText;
            }
            set
            {
                CommandText = value;
            }
        }
        public override int CommandTimeout
        {
            get
            {
                return CommandTimeout;
            }
            set
            {
                CommandTimeout = value;
            }
        }
        public override CommandType CommandType
        {
            get
            {
                return CommandType;
            }
            set
            {
                CommandType = value;
            }
        }
        public override bool DesignTimeVisible
        {
            get
            {
                return DesignTimeVisible;
            }
            set
            {
                DesignTimeVisible = value;
            }
        }
        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                return UpdatedRowSource;
            }
            set
            {
                UpdatedRowSource = value;
            }
        }
        protected override DbConnection DbConnection
        {
            get
            {
                return DbConnection;
            }
            set
            {
                DbConnection = value;
            }
        }

        protected override DbParameterCollection DbParameterCollection => throw new NotImplementedException();

        protected override DbTransaction DbTransaction
        {
            get
            {
                return DbTransaction;
            }
            set
            {
                DbTransaction = value;
            }
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }
    }
}
