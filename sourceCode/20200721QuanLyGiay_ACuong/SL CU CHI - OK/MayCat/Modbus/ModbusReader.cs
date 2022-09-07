using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public abstract class ModbusReader
    {
        public ModbusReader(List<Tag> tagSource, ByteOrder byteOrder = ByteOrder.CDAB)
        {
            TagSource = tagSource.OrderBy(x => x.Address).ToList();
            ByteOrder = byteOrder;
        }

        public virtual ByteOrder ByteOrder { get; set; }
        public virtual List<Tag> TagSource { get; set; }

        public virtual void Start()
        {

        }

        public virtual bool WriteRegisters(uint address, byte[] writeValues)
        {
            return false;
        }

        public virtual bool WriteTag(Tag tag, object value)
        {
            return false;
        }
    }
}
