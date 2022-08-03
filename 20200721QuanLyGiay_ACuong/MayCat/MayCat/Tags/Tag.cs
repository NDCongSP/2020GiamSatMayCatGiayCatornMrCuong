using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public class Tag : INotifyPropertyChanged
    {
        public ModbusReader ModbusReader { get; set; }

        public Tag(string name, uint address, DataType dataType)
        {
            Name = name;
            Address = address;
            DataType = dataType;
            if (DecomposeAddress(Address.ToString(), out AddressType addressType, out ushort addressOffset))
            {
                AddressType = addressType;
                AddressOffset = addressOffset;
            }
        }

        public ByteOrder ByteOrder { get; set; }
        public string Name { get; set; }
        public uint Address { get; set; }
        public AddressType AddressType { get; set; }
        public ushort AddressOffset { get; set; }
        public DataType DataType { get; set; }

        public IDataType DataTypeBase { get; set; }

        private Quality quality;
        public Quality Quality
        {
            get => quality;
            set
            {
                if (quality != value)
                {
                    var oldValue = quality;
                    quality = value;
                    RaisePropertyChanged();
                    QualityChanged?.Invoke(this, oldValue, value);
                }
            }
        }

        private double value;
        public double Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    var oldValue = this.value;
                    this.value = value;
                    ValueChanged?.Invoke(this, oldValue, value);
                    RaisePropertyChanged();
                }
            }
        }

        public double Gain { get; set; } = 1;
        public double Offset { get; set; } = 0;
        public int IndexInBuffer { get; set; }

        public int GetByteLength()
        {
            switch (DataType)
            {
                case DataType.Word:
                case DataType.Short:
                    return 2;
                case DataType.DWord:
                case DataType.Long:
                    return 4;
                default:
                    break;
            }
            return 0;
        }



        public static bool DecomposeAddress(string address, out AddressType addressType, out ushort offset)
        {
            addressType = AddressType.Undefined;
            offset = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (uint.TryParse(address, out uint adrNumber))
                    {
                        int type = (int)(adrNumber / 100000);
                        int odd = (int)(adrNumber % 100000) - 1;

                        if (Enum.TryParse(type.ToString(), out addressType) && odd >= 0 && (ushort)odd <= (ushort)0xFFFFU)
                        {
                            offset = (ushort)odd;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch { return false; }
        }

        public event Action<Tag, double, double> ValueChanged;
        public event Action<Tag, Quality, Quality> QualityChanged;

        public bool Write(object value)
        {
            return ModbusReader.WriteTag(this, value);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
