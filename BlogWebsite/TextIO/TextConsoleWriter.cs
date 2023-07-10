using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsite.TextIO
{
    public class TextConsoleWriter : TextWriter
    {
        private StreamWriter logging;
        private System.Windows.Forms.RichTextBox _textBox { set; get; }
        private int maxRowLenght = 0;//textBox中显示的最大行数，若不限制，则置为0
        public TextConsoleWriter(System.Windows.Forms.RichTextBox txt)
        {
            _textBox = txt;
            Console.SetOut(this);
            logging = new StreamWriter(new FileStream("log.txt", FileMode.OpenOrCreate));
        }
        ~TextConsoleWriter()
        {
            try
            {
                logging.Close();
                logging.Dispose();
            }
            catch (Exception) { }
        }
        public override void Write(string value)
        {
            if (_textBox.IsHandleCreated)
                _textBox.Invoke(new ThreadStart(() =>
                {
                    _textBox.AppendText(value);
                    TextConsoleReader.bufferedlength += value.Length;
                }));
            else
                _textBox.BeginInvoke(new ThreadStart(() =>
                {
                    _textBox.AppendText(value);
                    TextConsoleReader.bufferedlength += value.Length;
                }));
            logging.Write(value);
        }

        public override void WriteLine(string value)
        {
            if (_textBox.IsHandleCreated)
                _textBox.Invoke(new ThreadStart(() =>
                {
                    _textBox.AppendText(value + "\r\n");
                    TextConsoleReader.bufferedlength += value.Length + 2;
                }));
            else
            {
                _textBox.BeginInvoke(new ThreadStart(() =>
                {
                    _textBox.AppendText(value + "\r\n");
                    TextConsoleReader.bufferedlength += value.Length + 2;
                }));
            }
            logging.WriteLine(value);
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
