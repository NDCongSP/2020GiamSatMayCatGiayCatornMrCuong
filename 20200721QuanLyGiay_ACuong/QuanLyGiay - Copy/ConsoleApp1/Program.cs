using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:32000");
            //    IDatabase db = redis.GetDatabase();
            //    //string value = "Hello";
            //    //db.StringSet("test", value);
            //            string restore = db.StringGet("test");
            //Console.WriteLine(restore);
            //    Console.ReadLine();
            Brush red = Brushes.Red;
            var brush = new BrushConverter();
            var redConvert = brush.ConvertFromString(red.ToString());
            Console.ReadLine();
        }
    }
}
