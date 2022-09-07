using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public enum AddressType
    {
        Undefined = int.MaxValue,
        OutputCoil = 0,
        InputContact = 1,
        InputRegister = 3,
        HoldingRegister = 4,
    }
}
