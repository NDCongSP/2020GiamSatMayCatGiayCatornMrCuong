using CommonControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCatServer
{
    public static class Helper
    {
        public static string ToHMSString(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }
    }
}
