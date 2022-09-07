using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCatServer
{
    public sealed class WriteCommand
    {
        public string PathToTag { get; set; }
        public string Value { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime ReceiveTime { get; set; }
    }
}
