using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BlogWebsite.DynamicCompiler;
using BlogWebsite.DynamicCompiler.Core;
using BlogWebsite.Server;

namespace BlogWebsite.ServerScript
{
    public class ScriptController
    {
        /// <summary>
        /// 正则表达式
        /// </summary>
        private static Regex regex = new Regex("(?<=<%)[^%]*(?=%>)", RegexOptions.Compiled);
        /// <summary>
        /// 获得ScriptController的唯一对象，由于使用了单例模式，固该对象常驻内存不会被GC回收
        /// </summary>
        public static ScriptController getController {get;private set;}
        /// <summary>
        /// 获取文本中的表达式，并执行
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string eval(Session session,string text)
        {
            string result = "";
            //正则表达式提取可执行的代码，用<% ... %>包围的文本就是可执行的代码
            MatchCollection mc = regex.Matches(text);
            int tmp = 0;
            foreach (Match m in mc)
            {
                result += text.Substring(tmp, m.Index - tmp - 2) + calc(session,m.Value);
                tmp = m.Index + m.Length + 2;
            }
            result += text.Substring(tmp, text.Length - tmp);
            CompilerCore.functionlist.Clear();
            return result;
        }
        /// <summary>
        /// 通过expression读取服务器端脚本
        /// 使用内置的脚本解释器计算表达式的值
        /// 表达式支持函数，但不支持数组
        /// 因为写完函数就忙着面试了，没空实现数组。
        /// </summary>
        /// <returns></returns>
        private static string calc(Session session,string code)
        {
            CompilerCore compiler = CompilerCore.GetCompiler();
            compiler.Lexer(code);//词法分析
            Function fun = compiler.arrange();//语法分析
            Data res = fun.run();
            string[] arrs = res.str.Split(':');
            string result = string.Empty;
            switch (arrs[0])
            {
                case "GET":
                    try
                    {
                        result = session.global_variables[arrs[1].Trim()].ToString();
                    }
                    catch(Exception e)
                    {
                        if(e is KeyNotFoundException || e is ArgumentNullException)
                            result = "null";
                    }
                    break;
                case "ATTACH":
                    try
                    {
                        FileLoader fl = FileLoader.GetFile(arrs[1].Trim());
                        result = fl.data;
                    }
                    catch (Exception ex)
                    {
                        result = "发生异常，无法加载文件:" + arrs[1].Trim();
                    }
                    break;
                default:
                    result = res.str;
                    break;
            }
            return result;
        }
    }
}
