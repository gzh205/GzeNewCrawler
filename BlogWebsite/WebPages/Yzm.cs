using BlogWebsite.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.WebPages
{
    [PageInfo(Name = "/yzm")]
    class Yzm : Page
    {
        public override Response get(Request request)
        {
            int[] vars = Get2Nums();
            session.global_variables["yzm"] = vars[0]+vars[1];
            byte[] data = CreateCode(vars[0], vars[1]);
            return new Server.response_impl.BinaryData(data);
        }

        public override Response post(Request request)
        {
            int[] vars = Get2Nums();
            session.global_variables["yzm"] = vars[0] + vars[1];
            byte[] data = CreateCode(vars[0], vars[1]);
            return new Server.response_impl.BinaryData(data);
        }
        public int[] Get2Nums()
        {
            Random r = new Random((int)System.DateTime.Now.Ticks);
            int var1 = r.Next(0, 50);
            int var2 = r.Next(0, 50);
            return new int[] { var1,var2 };
        }
        public byte[] CreateCode(int var1,int var2)
        {
            string varifycode = var1 + "+" + var2;
            return Tools.ValidateCodeHelper.CreateValidateGraphic(varifycode);
        }
    }
}
