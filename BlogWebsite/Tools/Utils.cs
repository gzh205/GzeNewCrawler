using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Tools
{
    public class Utils
    {
        public static string MyInt2String(int len)
        {
            uint len_ = (uint)len;
            char[] len_chars= new char[sizeof(int)];
            //将整数len转换为4字节的char数组
            for(int i = 0; i < sizeof(int); i++)
            {
                len_chars[i] = (char)(len_ % 0x100);
                len_ = len_ / 0x100;
            }
            return new string(len_chars);
        }
    }
}
