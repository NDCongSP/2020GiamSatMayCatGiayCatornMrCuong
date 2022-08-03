using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BangLed
{
    class fileText
    {
        public void write_text(string noidung,string dgdan)
        {
            FileStream fs = new FileStream(dgdan, FileMode.Create, FileAccess.Write, FileShare.None);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
            sw.WriteLine(noidung);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        public string read_text( string dgdan)
        {
            try
            {
                FileStream fs = new FileStream(dgdan, FileMode.Open, FileAccess.Read, FileShare.None);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);
                string text = sr.ReadToEnd();
                sr.Close();
                fs.Close();
                return text;
            }
            catch { return "NULL"; }
        }
    }
}
