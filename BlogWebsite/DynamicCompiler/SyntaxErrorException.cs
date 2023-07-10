using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWebsite.DynamicCompiler
{
    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string msg) : base(msg) { }
    }
}
