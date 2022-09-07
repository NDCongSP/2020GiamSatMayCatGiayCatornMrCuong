using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MayCat
{
    public class ModbusRTUReader : ModbusReader
    {
        public ModbusRTUReader(string configStr, byte deviceId, List<Tag> tagSource, ByteOrder byteOrder = ByteOrder.CDAB) : base(tagSource, byteOrder)
        {
            this.deviceId = deviceId;
            this.configStr = configStr;

            string[] split = configStr.Split('.');
            portName = split[0];
            baudRate = int.Parse(split[1]);
            dataBits = int.Parse(split[2]);
            foreach (var item in Enum.GetValues(typeof(Parity)))
            {
                if (item.ToString().StartsWith(split[3]))
                {
                    parity = (Parity)item;
                    break;
                }    
            }

            stopBits = (StopBits)(int.Parse(split[4]));
        }

        private string configStr;
        private byte deviceId;
        private DispatcherTimer timer;
        private SemaphoreSlim locker = new SemaphoreSlim(1, 1);
        private ModbusSerialRTU master;
        public List<ReadBlock> Blocks { get; set; } = new List<ReadBlock>();
        string portName;
        int baudRate;
        int dataBits;
        StopBits stopBits = StopBits.One;
        Parity parity = Parity.None;

        private Word WordType { get; set; } = new Word();
        private DWord DWordType { get; set; } = new DWord();
        private Int ShortType { get; set; } = new Int();
        private DInt LongType { get; set; } = new DInt();

        public override void Start()
        {
            try
            {
                InitMbClient();
                ReadBlock currentBlock = null;
                foreach (Tag tag in TagSource)
                {
                    tag.ModbusReader = this;
                    if (currentBlock == null)
                    {
                        currentBlock = new ReadBlock();
                        currentBlock.TryAdd(tag);
                    }
                    else
                    {
                        if (!currentBlock.TryAdd(tag))
                        {
                            Blocks.Add(currentBlock);
                            currentBlock = new ReadBlock();
                            currentBlock.TryAdd(tag);
                        }
                    }
                    tag.ByteOrder = ByteOrder;
                    IDataType dataType = null;
                    switch (tag.DataType)
                    {
                        case DataType.Word:
                            dataType = WordType;
                            break;
                        case DataType.DWord:
                            dataType = DWordType;
                            break;
                        case DataType.Short:
                            dataType = ShortType;
                            break;
                        case DataType.Long:
                            dataType = LongType;
                            break;
                        default:
                            break;
                    }
                    tag.DataTypeBase = dataType;
                }

                if (currentBlock != null && currentBlock.RegisterTags.Count > 0)
                {
                    Blocks.Add(currentBlock);
                }

                foreach (var item in Blocks)
                {
                    item.Init(); ;
                }

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(50);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            catch { }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            try
            {
                InitMbClient();

                if (master.Open() && TagSource != null)
                {
                    foreach (var block in Blocks)
                    {
                        block.ReadResult = false;
                        int readCount = 0;
                        while (!block.ReadResult)
                        {
                            switch (block.AddressType)
                            {
                                case AddressType.InputRegister:
                                case AddressType.HoldingRegister:
                                    block.ReadResult = ReadRegisters(deviceId, block.AddressType, block.StartOffset, block.WordCount, ref block.ByteBuffer);
                                    break;
                                default:
                                    break;
                            }
                            readCount++;
                            if (readCount >= 3)
                                break;
                        }

                        if (block.ReadResult)
                        {
                            switch (block.AddressType)
                            {
                                case AddressType.InputRegister:
                                case AddressType.HoldingRegister:
                                    {
                                        foreach (var tag in block.RegisterTags)
                                        {
                                            tag.Quality = Quality.Good;
                                            UpdateValue(block.ByteBuffer, tag);
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            foreach (var tag in block.RegisterTags)
                            {
                                tag.Quality = Quality.Bad;
                            }
                        }
                    }
                }
            }
            catch
            {
                foreach (var tag in TagSource)
                {
                    tag.Quality = Quality.Bad;
                }
            }
            finally
            {
                timer.Start();
            }
        }

        private void UpdateValue(byte[] buffer, Tag tag)
        {
            string value = null;
            value = tag.DataTypeBase.ConvertToValue(buffer, tag.Gain, tag.Offset, tag.IndexInBuffer, 0, ByteOrder);
            if (double.TryParse(value, out double dValue))
            {
                tag.Value = dValue;
            }
        }

        /// <summary>
        /// Hàm đọc input register và holding register
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="addressType"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="byteBuffer"></param>
        /// <returns></returns>
        private bool ReadRegisters(byte unitId, AddressType addressType, ushort offset, ushort count, ref byte[] byteBuffer)
        {
            try
            {
                locker.Wait();
                switch (addressType)
                {
                    case AddressType.InputRegister:
                        return master.ReadInputRegisters(unitId, offset, count, ref byteBuffer);
                    case AddressType.HoldingRegister:
                        return master.ReadHoldingRegisters(unitId, offset, count, ref byteBuffer);
                    default:
                        break;
                }
                return false;
            }
            catch { return false; }
            finally { Thread.Sleep(3); locker.Release(); }
        }

        public override bool WriteRegisters(uint address, byte[] writeValues)
        {
            locker.Wait();
            try
            {
                if (Tag.DecomposeAddress(address.ToString(), out AddressType adrType, out ushort offset))
                {
                    if (master != null)
                    {
                        if (adrType == AddressType.HoldingRegister)
                        {
                            return master.WriteHoldingRegisters(deviceId, offset, (ushort)(writeValues.Length / 2), writeValues);
                        }
                    }
                }
            }
            catch { }
            finally { locker.Release(); }
            return false;
        }

        public override bool WriteTag(Tag tag, object value)
        {
            locker.Wait();
            try
            {
                if (TagSource.Contains(tag) && tag.AddressType == AddressType.HoldingRegister)
                {


                    if (tag.DataTypeBase != null)
                    {
                        if (tag.DataTypeBase.TryParseToByteArray(value, tag.Gain, tag.Offset, out byte[] writeValues, ByteOrder))
                        {

                            if (master != null)
                            {
                                return master.WriteHoldingRegisters(deviceId, tag.AddressOffset, (ushort)(tag.GetByteLength() / 2), writeValues);
                            }
                        }
                    }
                }
            }
            catch { }
            finally { locker.Release(); }
            return false;
        }

        public void InitMbClient()
        {
            locker.Wait();
            try
            {
                if (master == null)
                    master = new ModbusSerialRTU();

                master.Init(portName, baudRate, dataBits, parity, stopBits);
                master.Open();
            }
            catch { }
            finally { locker.Release(); }

        }
    }
}
