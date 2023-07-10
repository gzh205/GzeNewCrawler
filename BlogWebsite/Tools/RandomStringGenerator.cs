using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Tools
{
    public class RandomStringGenerator
    {
        public static string Generate(int count)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            string randomString = "";
            for (int i = 0; i < count; i++)
            {
                randomString += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))).ToString();
            }
            return randomString;
        }
    }
}
