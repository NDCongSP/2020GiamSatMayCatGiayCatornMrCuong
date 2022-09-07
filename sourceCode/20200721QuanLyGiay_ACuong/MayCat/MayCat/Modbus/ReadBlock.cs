using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public class ReadBlock
    {
        public List<Tag> RegisterTags { get; set; }
        public AddressType AddressType { get; set; }
        public ushort StartOffset { get; set; }
        public ushort EndOffset { get; set; }
        public bool ReadResult { get; set; }

        public ushort WordCount { get => (ushort)(EndOffset - StartOffset + 1); }

        public byte[] ByteBuffer;
        public bool[] BoolBuffer;

        public ReadBlock()
        {
            RegisterTags = new List<Tag>();
        }

        public void Init()
        {
            ByteBuffer = new byte[(EndOffset - StartOffset + 1) * 2];
        }

        public bool TryAdd(Tag tag)
        {
            if (tag != null)
            {
                if (RegisterTags.Count == 0)
                {
                    AddressType = tag.AddressType;
                    StartOffset = tag.AddressOffset;
                    tag.IndexInBuffer = 0;
                    RegisterTags.Add(tag);
                    return true;
                }
                else
                {
                    if (AddressType == tag.AddressType)
                    {
                        int endOffset = tag.AddressOffset + (tag.GetByteLength() / 2) - 1;

                        if (endOffset - StartOffset <= 120 && endOffset - StartOffset > 0)
                        {
                            EndOffset = (ushort)endOffset;
                            tag.IndexInBuffer = (tag.AddressOffset - StartOffset) * 2;
                            RegisterTags.Add(tag);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

}
