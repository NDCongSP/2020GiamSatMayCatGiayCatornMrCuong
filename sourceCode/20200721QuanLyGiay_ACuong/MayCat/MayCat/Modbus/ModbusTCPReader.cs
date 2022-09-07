using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MayCat
{
    public class ModbusTCPReader : ModbusReader
    {
        public ModbusTCPReader(string ipAddress, byte deviceId, List<Tag> tagSource, ByteOrder byteOrder = ByteOrder.CDAB) : base(tagSource, byteOrder)
        {
            this.ipAddress = ipAddress;
            this.deviceId = deviceId;
        }

        private string ipAddress;
        private byte deviceId;
        private ModbusTCPClient mbClient;
        private DispatcherTimer timer;
        private SemaphoreSlim locker = new SemaphoreSlim(1, 1);
        public List<ReadBlock> Blocks { get; set; } = new List<ReadBlock>();

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
                if (mbClient == null)
                    InitMbClient();

                if (!mbClient.Connected)
                {
                    try
                    {
                        locker.Wait();
                        mbClient.Connect(ipAddress, 502, false);
                    }
                    catch { }
                    finally { locker.Release(); }
                }

                if (mbClient.IsSocketNull)
                {
                    try
                    {
                        InitMbClient();
                    }
                    catch { }
                }

                if (mbClient != null && mbClient.Connected && TagSource != null)
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
                if (mbClient.IsSocketNull)
                    InitMbClient();

                if (mbClient == null || mbClient.IsSocketNull)
                {
                    return false;
                }

                locker.Wait();
                byte[] result = null;
                switch (addressType)
                {
                    case AddressType.InputRegister:
                        mbClient.ReadInputRegister(unitId, unitId, offset, count, ref result);
                        break;
                    case AddressType.HoldingRegister:
                        mbClient.ReadHoldingRegister(unitId, unitId, offset, count, ref result);
                        break;
                    default:
                        break;
                }
                if (result != null)
                {
                    Array.Copy(result, byteBuffer, byteBuffer.Length);
                    return true;
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
                    if (mbClient != null && !mbClient.IsSocketNull && mbClient.Connected)
                    {
                        if (adrType == AddressType.HoldingRegister)
                        {
                            byte[] result = null;
                            mbClient.WriteMultipleRegister(deviceId, deviceId, offset, writeValues, ref result);
                            if (result == null)
                                return false;
                            return true;
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

                            if (mbClient != null && !mbClient.IsSocketNull && mbClient.Connected)
                            {
                                byte[] result = null;
                                mbClient.WriteMultipleRegister(deviceId, deviceId, tag.AddressOffset, writeValues, ref result);
                                if (result == null)
                                    return false;
                                return true;

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
                if (mbClient != null)
                    mbClient.Dispose();
                mbClient = new ModbusTCPClient();
                mbClient.Connect(ipAddress, 502, false);
            }
            catch { }
            finally { locker.Release(); }

        }
    }

}
