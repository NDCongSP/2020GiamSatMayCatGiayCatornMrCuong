using CommonControls;
using EasyDriverPlugin;
using MySql.Data.MySqlClient.Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MayCatServer
{
    public class MayXaController
    {
        public static MayXaController Instance { get; } = new MayXaController();

        #region Private members
        private int _slDonHang1LanNap = 5;
        internal string _comPort = "COM3";
        private int _baudrate = 9600;
        private StopBits _stopBits = StopBits.One;
        private int _dataBits = 8;
        private Parity _parity = Parity.None;
        private object _locker = new object();

        private byte _deviceId = 0x03;
        private byte _funCode = 0x10;
        private int _startAddress = 0x0001;
        private int _memoryCount = 0x0A5;
        private int _startMark = 0xFF;
        #endregion

        public MayXaController()
        {
        }

        #region Public properties

        public void NapDon(List<DonHang> donHangs, int startStt, int mayXa)
        {
            try
            {
                if (donHangs != null)
                {
                    // CutterTags.Instance.LenhChuyenDon?.Write("0");
                    // Lọc các đơn chưa hoàn thành cutter và có stt lớn hơn STT đang chạy
                    List<DonHang> DanhSachNap = new List<DonHang>();

                    foreach (var item in donHangs.Where(x =>
                        x.STT > startStt && x.MayXa == mayXa).OrderBy(x => x.STT))
                    {
                        if (DanhSachNap.Count < _slDonHang1LanNap)
                        {
                            DanhSachNap.Add(item);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (DanhSachNap != null)
                    {
                        DonHangBuilder builder = new DonHangBuilder();
                        foreach (var item in DanhSachNap)
                        {
                            builder.ThemDonHang(item.Xa, item.Rong, item.Cao, item.Canh, item.Song, item.Lang);
                        }

                        lock (_locker)
                        {
                            using (SerialController serialController = new SerialController())
                            {
                                serialController.Init(_comPort, _baudrate, _dataBits, _parity, _stopBits);
                                if (serialController.Open())
                                {
                                    byte[] content = builder.Build();

                                    byte[] headerBuffer = new byte[7];
                                    headerBuffer[0] = _deviceId;
                                    headerBuffer[1] = _funCode;
                                    headerBuffer[2] = (byte)(_startAddress >> 8);
                                    headerBuffer[3] = (byte)(_startAddress);
                                    headerBuffer[4] = (byte)(_memoryCount >> 8);
                                    headerBuffer[5] = (byte)(_memoryCount);
                                    headerBuffer[6] = (byte)(_startMark);

                                    byte[] frame = new byte[headerBuffer.Length + content.Length + 2]; // Add 2 for CRCs
                                    Buffer.BlockCopy(headerBuffer, 0, frame, 0, headerBuffer.Length);
                                    Buffer.BlockCopy(content, 0, frame, headerBuffer.Length, content.Length);

                                    byte[] response = serialController.SendMessage(frame, 8);
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public void NapDon2(List<DonHang> donHangs, int startStt, int mayXa)
        {
            try
            {
                if (donHangs != null)
                {
                    // CutterTags.Instance.LenhChuyenDon?.Write("0");
                    // Lọc các đơn chưa hoàn thành cutter và có stt lớn hơn STT đang chạy
                    List<DonHang> DanhSachNap = new List<DonHang>();

                    foreach (var item in donHangs.Where(x =>
                        x.STT > startStt && x.MayXa == mayXa).OrderBy(x => x.STT))
                    {
                        if (DanhSachNap.Count < _slDonHang1LanNap)
                        {
                            DanhSachNap.Add(item);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (DanhSachNap != null)
                    {
                        DonHangBuilder2 builder = new DonHangBuilder2();
                        foreach (var item in DanhSachNap)
                        {
                            builder.ThemDonHang(item.Xa, item.Rong, item.Cao, item.Canh, item.Song, item.Lang);
                        }

                        lock (_locker)
                        {
                            using (SerialController serialController = new SerialController())
                            {
                                serialController.Init(_comPort, _baudrate, _dataBits, _parity, _stopBits);
                                if (serialController.Open())
                                {
                                    byte[] content = builder.Build();

                                    byte[] headerBuffer = new byte[2];
                                    headerBuffer[0] = 0x02;
                                    headerBuffer[1] = 0x4F;

                                    byte[] frame = new byte[headerBuffer.Length + content.Length + 2]; // Add 2 for CRCs + 1 fore end byte
                                    Buffer.BlockCopy(headerBuffer, 0, frame, 0, headerBuffer.Length);
                                    Buffer.BlockCopy(content, 0, frame, headerBuffer.Length, content.Length);

                                    byte[] response = serialController.SendMessage(frame, 5, 0x03);

                                    string hex = BitConverter.ToString(response).Replace("-", " ");
                                    MessageBox.Show(hex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }

    class SerialController : IDisposable
    {
        public SerialPort SerialPort { get; private set; } = new SerialPort();
        public string modbusStatus;
        public int ResponseTimeOut { get; set; } = 3000;

        #region Constructor / Deconstructor
        public SerialController()
        {
        }
        #endregion

        #region Open / Close Procedures
        public void Init(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            //Assign desired settings to the serial port:
            if (!SerialPort.IsOpen)
                SerialPort.PortName = portName;
            else
            {
                if (portName != SerialPort.PortName)
                {
                    SerialPort.Close();
                    SerialPort.PortName = portName;
                }
            }
            SerialPort.BaudRate = baudRate;
            SerialPort.DataBits = databits;
            SerialPort.Parity = parity;
            SerialPort.ReadTimeout = ResponseTimeOut;
            SerialPort.WriteTimeout = ResponseTimeOut;
            if (stopBits != StopBits.None)
                SerialPort.StopBits = stopBits;
        }

        public bool Open()
        {
            //Ensure port isn't already opened:
            if (!SerialPort.IsOpen)
            {
                //These timeouts are default and cannot be editted through the class at this point:
                SerialPort.ReadTimeout = ResponseTimeOut;
                SerialPort.WriteTimeout = ResponseTimeOut;

                try
                {
                    SerialPort.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + SerialPort.PortName + ": " + err.Message;
                    return false;
                }
                modbusStatus = SerialPort.PortName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = SerialPort.PortName + " already opened";
                return true;
            }
        }
        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (SerialPort.IsOpen)
            {
                try
                {
                    SerialPort.Close();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error closing " + SerialPort.PortName + ": " + err.Message;
                    return false;
                }
                modbusStatus = SerialPort.PortName + " closed successfully";
                return true;
            }
            else
            {
                modbusStatus = SerialPort.PortName + " is not open";
                return false;
            }
        }
        #endregion

        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {

            //SEE CRC.XLS DOCUMENTATION//

            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                //XOR CRCfull with 16bits message to compute it's CRC
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    //get LSB
                    CRCLSB = (char)(CRCFull & 0x0001);

                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);


        }
        #endregion

        #region Build Message
        private void BuildMessage(byte deviceId, byte type, ushort start, ushort registers, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = deviceId;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];

        }
        #endregion

        public byte[] SendMessage(byte[] buffer, int responseLen, byte? endByte = null)
        {
            if (SerialPort.IsOpen)
            {
              
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();

                byte[] CRC = new byte[2];
                GetCRC(buffer, ref CRC);

                buffer[buffer.Length - 2] = CRC[0];
                buffer[buffer.Length - 1] = CRC[1];

                bool isSend = true;
                byte[] response = new byte[responseLen];
                try
                {

                    if (endByte.HasValue)
                    {
                        byte[] sendBuffer = new byte[buffer.Length + 1];
                        Array.Copy(buffer, 0, sendBuffer, 0, buffer.Length);
                        sendBuffer[sendBuffer.Length - 1] = endByte.Value;
                        SerialPort.Write(sendBuffer, 0, sendBuffer.Length);
                        string hex = BitConverter.ToString(sendBuffer).Replace("-", " ");
                        MessageBox.Show(hex);
                        isSend = false;
                    }
                    else
                    {
                        SerialPort.Write(buffer, 0, buffer.Length);
                    }

                    GetResponse(ref response);
                    return response;
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    MessageBox.Show(err.ToString());
                }
            }
            return null;
        }

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion

        #region Get Response
        private void GetResponse(ref byte[] response)
        {
            //There is a bug in .Net 2.0 DataReceived Event that prevents people from using this
            //event as an interrupt to handle data (it doesn't fire all of the time).  Therefore
            //we have to use the ReadByte command for a fixed length as it's been shown to be reliable.
            for (int i = 0; i < response.Length; i++)
            {
                response[i] = (byte)SerialPort.ReadByte();
            }
        }
        #endregion

        #region Function 16 - Write Multiple Registers
        public bool WriteHoldingRegisters(byte deviceId, ushort start, ushort registers, byte[] values)
        {
            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                byte[] message = new byte[9 + 2 * registers];
                //Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Add bytecount to message:
                message[6] = (byte)(registers * 2);
                //Put write values into message prior to sending:
                for (int i = 0; i < registers; i++)
                {
                    message[7 + 2 * i] = values[2 * i];
                    message[8 + 2 * i] = values[2 * i + 1];
                }
                //Build outgoing message:
                BuildMessage(deviceId, (byte)16, start, registers, ref message);

                //Send Modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function 3 - Read Holding Registers
        public bool ReadHoldingRegisters(byte deviceId, ushort start, ushort registers, ref byte[] values)
        {
            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //Function 3 request is always 8 bytes:
                byte[] message = new byte[8];
                //Function 3 response buffer:
                byte[] response = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(deviceId, (byte)3, start, registers, ref message);
                //Send modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    //Return requested register values:
                    for (int i = 0; i < (response.Length - 5) / 2; i++)
                    {
                        values[2 * i] = response[2 * i + 3];
                        //values[i] <<= 8;
                        values[2 * i + 1] = response[2 * i + 4];
                    }
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }
        #endregion

        #region Function 05 - Write Single Coil
        public bool WriteSingleCoil(byte deviceId, ushort start, bool value)
        {
            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //Message is 1 addr + 1 fcn + 2 start + 2 status to write + 2 CRC
                byte[] message = new byte[8];
                //Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Array to receive CRC bytes:
                byte[] CRC = new byte[2];

                message[0] = deviceId;
                message[1] = 5;
                message[2] = (byte)(start >> 8);
                message[3] = (byte)start;

                if (value == true)
                    message[4] = (byte)(0xFF);
                else
                    message[4] = 0;

                message[5] = 0;

                GetCRC(message, ref CRC);
                message[message.Length - 2] = CRC[0];
                message[message.Length - 1] = CRC[1];

                //Send Modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    for (int i = 0; i < response.Length; i++)
                    {
                        if (response[i] != message[i])
                        {
                            modbusStatus = "Wrong reSerialPortonse";
                            return false;
                        }
                    }
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        public bool WriteSingleCoil(byte deviceId, ushort start, byte value)
        {
            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //Message is 1 addr + 1 fcn + 2 start + 2 status to write + 2 CRC
                byte[] message = new byte[8];
                //Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Array to receive CRC bytes:
                byte[] CRC = new byte[2];

                message[0] = deviceId;
                message[1] = 5;
                message[2] = (byte)(start >> 8);
                message[3] = (byte)start;
                message[4] = value;
                message[5] = 0;

                GetCRC(message, ref CRC);
                message[message.Length - 2] = CRC[0];
                message[message.Length - 1] = CRC[1];

                //Send Modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    for (int i = 0; i < response.Length; i++)
                    {
                        if (response[i] != message[i])
                        {
                            modbusStatus = "Wrong reSerialPortonse";
                            return false;
                        }
                    }
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function 15 - Write Multiple Coils
        public bool WriteMultipleCoils(byte deviceId, ushort start, ushort coils, bool[] values)
        {
            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();

                //Message is 1 addr + 1 fcn + 2 startcoil + 2 coilsnum  + 1 dbytes + dbytes * bytevals + 2 CRC

                int dbytes = 0;
                if ((coils % 8) > 0)
                    dbytes = coils / 8 + 1;
                else
                    dbytes = coils / 8;

                byte[] message = new byte[9 + dbytes];
                //Function 15 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Array to receive CRC bytes:
                byte[] CRC = new byte[2];

                message[0] = deviceId;
                message[1] = 15;
                message[2] = (byte)(start >> 8);
                message[3] = (byte)start;
                message[4] = (byte)(coils >> 8);
                message[5] = (byte)coils;
                message[6] = (byte)dbytes;

                int k = 0;
                if ((coils / 8) < dbytes)
                    k = dbytes * 8 - coils;


                //lay byte chan truoc		

                for (int i = 0; i < coils / 8; i++)
                {
                    message[7 + i] = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        message[7 + i] = Convert.ToByte(Convert.ToByte(values[8 * i + 7 - j]) | (message[7 + i] << 1));
                    }
                }

                //xu ly le
                for (int i = coils / 8; i < dbytes; i++)
                {

                    message[7 + i] = 0;

                    for (int j = k; j < 8; j++)
                    {
                        message[7 + i] = Convert.ToByte(Convert.ToByte(values[8 * i + 7 - j]) | (message[7 + i] << 1));
                    }
                }

                GetCRC(message, ref CRC);
                message[message.Length - 2] = CRC[0];
                message[message.Length - 1] = CRC[1];


                //Send Modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (response[i] != message[i])
                        {
                            modbusStatus = "Bad";
                            return false;
                        }
                    }
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function 1 - Read Coil Status
        public bool ReadCoils(byte deviceId, ushort start, ushort coils, ref bool[] values)
        {

            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //Function 1 request is always 8 bytes:
                byte[] message = new byte[8];

                //Array to receive CRC bytes:
                byte[] CRC = new byte[2];

                message[0] = deviceId;
                message[1] = 1;
                message[2] = (byte)(start >> 8);
                message[3] = (byte)start;
                message[4] = (byte)(coils >> 8);
                message[5] = (byte)coils;

                GetCRC(message, ref CRC);
                message[message.Length - 2] = CRC[0];
                message[message.Length - 1] = CRC[1];

                //Function 1 response buffer:
                //Frame: 1add + 1func + 1dbytes + dbytes*data + 2CRC

                int dbytes = 0;
                if ((coils % 8) > 0)
                    dbytes = coils / 8 + 1;
                else
                    dbytes = coils / 8;
                byte[] response = new byte[5 + dbytes];
                //Send modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    //Return requested register values:
                    for (int i = 0; i < (response.Length - 5); i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if ((8 * i + j) < coils)
                                values[8 * i + j] = Convert.ToBoolean((response[3 + i] >> j) & 0x01);
                        }
                    }
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }
        #endregion

        #region Function 2 - Read Discrete Input Contacts 1xxxx
        public bool ReadDiscreteInputContact(byte deviceId, ushort start, ushort inputs, ref bool[] values)
        {
            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //Function 2 request is always 8 bytes:
                byte[] message = new byte[8];

                //Array to receive CRC bytes:
                byte[] CRC = new byte[2];

                message[0] = deviceId;
                message[1] = 2;
                message[2] = (byte)(start >> 8);
                message[3] = (byte)start;
                message[4] = (byte)(inputs >> 8);
                message[5] = (byte)inputs;

                GetCRC(message, ref CRC);
                message[message.Length - 2] = CRC[0];
                message[message.Length - 1] = CRC[1];

                //Function 2 response buffer:
                //Frame: 1add + 1func + 1dbytes + dbytes*data + 2CRC

                int dbytes = 0;
                if ((inputs % 8) > 0)
                    dbytes = inputs / 8 + 1;
                else
                    dbytes = inputs / 8;
                byte[] response = new byte[5 + dbytes];
                //Send modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    //Return requested register values:
                    for (int i = 0; i < (response.Length - 5); i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if ((8 * i + j) < inputs)
                                values[8 * i + j] = Convert.ToBoolean((response[3 + i] >> j) & 0x01);
                        }
                    }
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }


        }
        #endregion

        #region Function 4 - Read Input Register 3xxxx
        public bool ReadInputRegisters(byte deviceId, ushort start, ushort registers, ref byte[] values)
        {
            //Ensure port is open:
            if (SerialPort.IsOpen)
            {
                //Clear in/out buffers:
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //Function 4 request is always 8 bytes:
                byte[] message = new byte[8];
                //Function 3 response buffer:
                byte[] response = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(deviceId, (byte)4, start, registers, ref message);
                //Send modbus message to Serial Port:
                try
                {
                    SerialPort.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }

                //Evaluate message:
                if (CheckResponse(response))
                {
                    //Return requested register values:
                    for (int i = 0; i < (response.Length - 5) / 2; i++)
                    {
                        values[2 * i] = response[2 * i + 3];
                        //values[i] <<= 8;
                        values[2 * i + 1] = response[2 * i + 4];
                    }
                    modbusStatus = "Good";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }

        public void Dispose()
        {
            SerialPort?.Close();
            SerialPort?.Dispose();
            GC.SuppressFinalize(SerialPort);
        }
        #endregion
    }

    class DonHangBuilder
    {
        int _slDonHang = 5;
        int _slWordCuaDonHang = 27 + 6;
        int count = 0;
        List<ushort[]> bufferList = new List<ushort[]>();
        
        public void ThemDonHang(int xa, int nap1, int cao, int nap2, string song, int lang)
        {
            if (count >= 5)
                return;

            if (nap1 < 0)
                nap1 = 0;
            if (cao < 0)
                cao = 0;
            if (nap2 < 0)
                nap2 = 0;

            if (lang < 0 || lang > 2)
                lang = 0;

            int dai = nap1 + cao + nap2;

            ushort[] wordArr = new ushort[_slWordCuaDonHang];

            wordArr[0] = (ushort)xa;
            wordArr[1] = (ushort)dai;
            wordArr[2] = (ushort)nap1;
            wordArr[3] = (ushort)cao;
            wordArr[4] = (ushort)nap2;

            wordArr[27] = GetSongValue(song);
            wordArr[28] = (ushort)lang;

            bufferList.Add(wordArr);
            count++;
        }

        public ushort GetSongValue(string song)
        {
            song = song?.Trim()?.ToUpper();
            // 2#0001-----A楞    2#0010----C楞    2#0100----B楞    2#1000----E楞
            if (!string.IsNullOrWhiteSpace(song) && song.Length <= 4)
            {
                ushort result = 0;
                song = song.PadLeft(4, ' ');
                for (int i = 0; i < song.Length; i++)
                {
                    int value = 0;
                    char currentChar = song[i];
                    switch (currentChar)
                    {
                        case 'A':
                            value = 0x01;
                            break;
                        case 'B':
                            value = 0x04;
                            break;
                        case 'C':
                            value = 0x02;
                            break;
                        case 'E':
                            value = 0x08;
                            break;
                        default:
                            break;
                    }
                    if (value != 0)
                    {
                        int count = (3 - i) * 4;
                        value = value << count;
                        result += (ushort)value;
                    }
                }

                if (result != 0)
                {
                    return result;
                }
            }
            return 0x0014;
        }

        public byte[] Build()
        {
            byte[] result = new byte[_slWordCuaDonHang * _slDonHang * 2];
            for (int i = 0; i < _slDonHang; i++)
            {
                if (i >= bufferList.Count)
                {
                    bufferList.Add(new ushort[_slWordCuaDonHang]);
                }
            }
            int offset = 0;
            for (int i = 0; i < bufferList.Count; i++)
            {
                ushort[] wordBuffer = bufferList[i];

                for (int j = 0; j < wordBuffer.Length; j++)
                {
                    result[offset + j * 2] = (byte)(wordBuffer[j] >> 8);
                    result[offset + j * 2 + 1] = (byte)(wordBuffer[j]);
                }

                offset += wordBuffer.Length * 2;
                // Buffer.BlockCopy(, 0, result, i * _slWordCuaDonHang * 2, bufferList[i].Length * 2);
            }
            return result;
        }
    }

    class DonHangBuilder2
    {
        int _slDonHang = 5;
        int _slWordCuaDonHang = 91;
        int count = 0;
        List<byte[]> bufferList = new List<byte[]>();

        public void ThemDonHang(int xa, int nap1, int cao, int nap2, string song, int lang)
        {
            if (count >= 5)
                return;

            if (nap1 < 0)
                nap1 = 0;
            if (cao < 0)
                cao = 0;
            if (nap2 < 0)
                nap2 = 0;

            if (lang < 0 || lang > 2)
                lang = 0;

            int dai = nap1 + cao + nap2;

            byte[] wordArr = new byte[_slWordCuaDonHang];

            // 0 - 6 Ma DonHang
            wordArr[6] = (byte)count;

            // 7 Kiểu sóng
            wordArr[7] = 0x31;
            // 8 Kiểu lằng
            wordArr[8] = 0x31;

            // 9 - 12 Chiều dài chặt
            ByteHelper.SetDWordAt(wordArr, 9, (uint)dai);

            // 13 Xả
            wordArr[13] = (byte)xa;

            // 14 - 17 Cắt rộng
            ByteHelper.SetDWordAt(wordArr, 14, (uint)nap1);

            // 18 - 21 Lằn ép 1
            ByteHelper.SetDWordAt(wordArr, 18, (uint)cao);

            // 22 - 25 Lằn ép 2
            ByteHelper.SetDWordAt(wordArr, 22, (uint)nap2);


            //// 14 - 17 Cắt rộng
            //ByteHelper.SetDWordAt(wordArr, 14, (uint)dai);

            //// 18 - 21 Lằn ép 1
            //ByteHelper.SetDWordAt(wordArr, 18, (uint)nap1);

            //// 22 - 25 Lằn ép 2
            //ByteHelper.SetDWordAt(wordArr, 22, (uint)cao);

            //// 26 - 29 Lằn ép 3
            //ByteHelper.SetDWordAt(wordArr, 26, (uint)nap2);

            bufferList.Add(wordArr);
            count++;
        }

        public ushort GetSongValue(string song)
        {
            song = song?.Trim()?.ToUpper();
            // 2#0001-----A楞    2#0010----C楞    2#0100----B楞    2#1000----E楞
            if (!string.IsNullOrWhiteSpace(song) && song.Length <= 4)
            {
                ushort result = 0;
                song = song.PadLeft(4, ' ');
                for (int i = 0; i < song.Length; i++)
                {
                    int value = 0;
                    char currentChar = song[i];
                    switch (currentChar)
                    {
                        case 'A':
                            value = 0x01;
                            break;
                        case 'B':
                            value = 0x04;
                            break;
                        case 'C':
                            value = 0x02;
                            break;
                        case 'E':
                            value = 0x08;
                            break;
                        default:
                            break;
                    }
                    if (value != 0)
                    {
                        int count = (3 - i) * 4;
                        value = value << count;
                        result += (ushort)value;
                    }
                }

                if (result != 0)
                {
                    return result;
                }
            }
            return 0x0014;
        }

        public byte[] Build()
        {
            byte[] result = new byte[_slWordCuaDonHang * _slDonHang];
            for (int i = 0; i < _slDonHang; i++)
            {
                if (i >= bufferList.Count)
                {
                    bufferList.Add(new byte[_slWordCuaDonHang]);
                }
            }

            int offset = 0;
            for (int i = 0; i < bufferList.Count; i++)
            {
                byte[] wordBuffer = bufferList[i];
                Buffer.BlockCopy(wordBuffer, 0, result, offset, wordBuffer.Length);
                offset += wordBuffer.Length;
            }
            return result;
        }
    }

    public enum ByteOrder
    {
        /// <summary>
        /// Big endian
        /// </summary>
        [Description("High byte first / High word first (Big endian)")]
        HighByte_HighWord,

        [Description("High byte first / Low word first")]
        HighByte_LowWord,

        [Description("Low byte first / High word first")]
        LowByte_HighWord,

        /// <summary>
        /// Little endian
        /// </summary>
        [Description("Low byte first / Low word first (Little endian)")]
        LowByte_LowWord,
    }

    public static class ByteHelper
    {
        #region [Help Functions]

        private static Int64 bias = 621355968000000000; // "decimicros" between 0001-01-01 00:00:00 and 1970-01-01 00:00:00

        public static int BCDtoByte(byte B)
        {
            return ((B >> 4) * 10) + (B & 0x0F);
        }

        public static byte ByteToBCD(int Value)
        {
            return (byte)(((Value / 10) << 4) | (Value % 10));
        }

        private static byte[] CopyFrom(byte[] Buffer, int Pos, int Size)
        {
            byte[] Result = new byte[Size];
            Array.Copy(Buffer, Pos, Result, 0, Size);
            return Result;
        }

        public static byte[] ConvertBoolArrayToByteArray(bool[] boolArray)
        {
            byte[] byteArr = new byte[(boolArray.Length + 7) / 8];
            BitArray arr = new BitArray(boolArray);
            arr.CopyTo(byteArr, 0);
            return byteArr;
        }

        public static bool[] ConvertByteArrayToBitArray(byte[] byteArray)
        {
            BitArray b = new BitArray(byteArray);
            bool[] bitValues = new bool[b.Count];
            b.CopyTo(bitValues, 0);
            Array.Reverse(bitValues);
            return bitValues;
        }

        #region Get/Set the bit at Pos.Bit
        public static bool GetBitAt(byte[] Buffer, int Pos, int Bit)
        {
            byte[] Mask = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
            if (Bit < 0) Bit = 0;
            if (Bit > 7) Bit = 7;
            return (Buffer[Pos] & Mask[Bit]) != 0;
        }
        public static void SetBitAt(ref byte[] Buffer, int Pos, int Bit, bool Value)
        {
            byte[] Mask = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
            if (Bit < 0) Bit = 0;
            if (Bit > 7) Bit = 7;

            if (Value)
                Buffer[Pos] = (byte)(Buffer[Pos] | Mask[Bit]);
            else
                Buffer[Pos] = (byte)(Buffer[Pos] & ~Mask[Bit]);
        }
        #endregion

        #region Get/Set 8 bit signed value (S7 SInt) -128..127
        public static int GetSIntAt(byte[] Buffer, int Pos)
        {
            int Value = Buffer[Pos];
            if (Value < 128)
                return Value;
            else
                return (int)(Value - 256);
        }
        public static void SetSIntAt(byte[] Buffer, int Pos, int Value)
        {
            if (Value < -128) Value = -128;
            if (Value > 127) Value = 127;
            Buffer[Pos] = (byte)Value;
        }
        #endregion

        #region Get/Set 16 bit signed value (S7 int) -32768..32767
        public static short GetIntAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                case ByteOrder.HighByte_LowWord:
                    return (short)((Buffer[Pos] << 8) | Buffer[Pos + 1]);
                case ByteOrder.LowByte_LowWord:
                case ByteOrder.LowByte_HighWord:
                    return (short)(Buffer[Pos] | Buffer[Pos + 1] << 8);
                default:
                    return default;
            }
        }
        public static void SetIntAt(byte[] Buffer, int Pos, Int16 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos] = (byte)(Value >> 8);
                    Buffer[Pos + 1] = (byte)(Value & 0x00FF);
                    break;
                case ByteOrder.LowByte_LowWord:
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 1] = (byte)(Value >> 8);
                    Buffer[Pos] = (byte)(Value & 0x00FF);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set 32 bit signed value (S7 DInt) -2147483648..2147483647
        public static int GetDIntAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                    {
                        int Result;
                        Result = Buffer[Pos]; Result <<= 8;
                        Result += Buffer[Pos + 1]; Result <<= 8;
                        Result += Buffer[Pos + 2]; Result <<= 8;
                        Result += Buffer[Pos + 3];
                        return Result;
                    }
                case ByteOrder.LowByte_HighWord:
                    {
                        int Result;
                        Result = Buffer[Pos + 1]; Result <<= 8;
                        Result += Buffer[Pos]; Result <<= 8;
                        Result += Buffer[Pos + 3]; Result <<= 8;
                        Result += Buffer[Pos + 2];
                        return Result;
                    }
                case ByteOrder.HighByte_LowWord:
                    {
                        int Result;
                        Result = Buffer[Pos + 2]; Result <<= 8;
                        Result += Buffer[Pos + 3]; Result <<= 8;
                        Result += Buffer[Pos]; Result <<= 8;
                        Result += Buffer[Pos + 1];
                        return Result;
                    }
                case ByteOrder.LowByte_LowWord:
                    {
                        int Result;
                        Result = Buffer[Pos + 3]; Result <<= 8;
                        Result += Buffer[Pos + 2]; Result <<= 8;
                        Result += Buffer[Pos + 1]; Result <<= 8;
                        Result += Buffer[Pos];
                        return Result;
                    }
                default:
                    return default;
            }

        }
        public static void SetDIntAt(byte[] Buffer, int Pos, int Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                    Buffer[Pos + 3] = (byte)(Value & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos] = (byte)((Value >> 24) & 0xFF);
                    break;
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 2] = (byte)(Value & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 24) & 0xFF);
                    break;
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos + 1] = (byte)(Value & 0xFF);
                    Buffer[Pos] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 24) & 0xFF);
                    break;
                case ByteOrder.LowByte_LowWord:
                    Buffer[Pos] = (byte)(Value & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 24) & 0xFF);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set 64 bit signed value (S7 LInt) -9223372036854775808..9223372036854775807
        public static Int64 GetLIntAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                    {
                        Int64 Result;
                        Result = Buffer[Pos]; Result <<= 8;
                        Result += Buffer[Pos + 1]; Result <<= 8;
                        Result += Buffer[Pos + 2]; Result <<= 8;
                        Result += Buffer[Pos + 3]; Result <<= 8;
                        Result += Buffer[Pos + 4]; Result <<= 8;
                        Result += Buffer[Pos + 5]; Result <<= 8;
                        Result += Buffer[Pos + 6]; Result <<= 8;
                        Result += Buffer[Pos + 7];
                        return Result;
                    }
                case ByteOrder.LowByte_LowWord:
                    {
                        Int64 Result;
                        Result = Buffer[Pos + 7]; Result <<= 8;
                        Result += Buffer[Pos + 6]; Result <<= 8;
                        Result += Buffer[Pos + 5]; Result <<= 8;
                        Result += Buffer[Pos + 4]; Result <<= 8;
                        Result += Buffer[Pos + 3]; Result <<= 8;
                        Result += Buffer[Pos + 2]; Result <<= 8;
                        Result += Buffer[Pos + 1]; Result <<= 8;
                        Result += Buffer[Pos];
                        return Result;
                    }
                default:
                    return default;
            }
        }
        public static void SetLIntAt(byte[] Buffer, int Pos, Int64 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                // HighByte_HighWord EFGH
                case ByteOrder.HighByte_HighWord:
                    Buffer[Pos + 7] = (byte)(Value & 0xFF);
                    Buffer[Pos + 6] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos] = (byte)((Value >> 56) & 0xFF);
                    break;
                // GHEF HighByte_LowWord
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos + 1] = (byte)(Value & 0xFF);
                    Buffer[Pos + 0] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 7] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos + 6] = (byte)((Value >> 56) & 0xFF);
                    break;
                // LowByte_HighWord FEHG
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 6] = (byte)(Value & 0xFF);
                    Buffer[Pos + 7] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 0] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 56) & 0xFF);
                    break;
                // HGFE LowByte_LogWord
                case ByteOrder.LowByte_LowWord:
                    Buffer[Pos] = (byte)(Value & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 6] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos + 7] = (byte)((Value >> 56) & 0xFF);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set 8 bit unsigned value (S7 USInt) 0..255
        public static byte GetUSIntAt(byte[] Buffer, int Pos)
        {
            return Buffer[Pos];
        }
        public static void SetUSIntAt(byte[] Buffer, int Pos, byte Value)
        {
            Buffer[Pos] = Value;
        }
        #endregion

        #region Get/Set 16 bit unsigned value (S7 UInt) 0..65535
        public static UInt16 GetUIntAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                case ByteOrder.HighByte_LowWord:
                    return (UInt16)((Buffer[Pos] << 8) | Buffer[Pos + 1]);
                case ByteOrder.LowByte_LowWord:
                case ByteOrder.LowByte_HighWord:
                    return (UInt16)((Buffer[Pos + 1] << 8) | Buffer[Pos]);
                default:
                    return default;
            }
        }
        public static void SetUIntAt(byte[] Buffer, int Pos, UInt16 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos] = (byte)(Value >> 8);
                    Buffer[Pos + 1] = (byte)(Value & 0x00FF);
                    break;
                case ByteOrder.LowByte_LowWord:
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 1] = (byte)(Value >> 8);
                    Buffer[Pos] = (byte)(Value & 0x00FF);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set 32 bit unsigned value (S7 UDInt) 0..4294967296
        public static UInt32 GetUDIntAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {

                case ByteOrder.HighByte_HighWord:
                    {
                        UInt32 Result;
                        Result = Buffer[Pos]; Result <<= 8;
                        Result |= Buffer[Pos + 1]; Result <<= 8;
                        Result |= Buffer[Pos + 2]; Result <<= 8;
                        Result |= Buffer[Pos + 3];
                        return Result;
                    }
                case ByteOrder.LowByte_HighWord:
                    {
                        UInt32 Result;
                        Result = Buffer[Pos + 1]; Result <<= 8;
                        Result |= Buffer[Pos]; Result <<= 8;
                        Result |= Buffer[Pos + 3]; Result <<= 8;
                        Result |= Buffer[Pos + 2];
                        return Result;
                    }
                case ByteOrder.HighByte_LowWord:
                    {
                        UInt32 Result;
                        Result = Buffer[Pos + 2]; Result <<= 8;
                        Result |= Buffer[Pos + 3]; Result <<= 8;
                        Result |= Buffer[Pos + 0]; Result <<= 8;
                        Result |= Buffer[Pos + 1];
                        return Result;
                    }
                case ByteOrder.LowByte_LowWord:
                    {
                        UInt32 Result;
                        Result = Buffer[Pos + 3]; Result <<= 8;
                        Result |= Buffer[Pos + 2]; Result <<= 8;
                        Result |= Buffer[Pos + 1]; Result <<= 8;
                        Result |= Buffer[Pos];
                        return Result;
                    }
                default:
                    return default;
            }
        }
        public static void SetUDIntAt(byte[] Buffer, int Pos, UInt32 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                    Buffer[Pos + 3] = (byte)(Value & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos] = (byte)((Value >> 24) & 0xFF);
                    break;
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 2] = (byte)(Value & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 24) & 0xFF);
                    break;
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos + 1] = (byte)(Value & 0xFF);
                    Buffer[Pos] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 24) & 0xFF);
                    break;
                case ByteOrder.LowByte_LowWord:
                    Buffer[Pos] = (byte)(Value & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 24) & 0xFF);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set 64 bit unsigned value (S7 ULint) 0..18446744073709551616
        public static UInt64 GetULIntAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                // HighByte_HighWord EFGH
                case ByteOrder.HighByte_HighWord:
                    {
                        UInt64 Result;
                        Result = Buffer[Pos]; Result <<= 8;
                        Result |= Buffer[Pos + 1]; Result <<= 8;
                        Result |= Buffer[Pos + 2]; Result <<= 8;
                        Result |= Buffer[Pos + 3]; Result <<= 8;
                        Result |= Buffer[Pos + 4]; Result <<= 8;
                        Result |= Buffer[Pos + 5]; Result <<= 8;
                        Result |= Buffer[Pos + 6]; Result <<= 8;
                        Result |= Buffer[Pos + 7];
                        return Result;
                    }
                // GHEF HighByte_LowWord
                case ByteOrder.HighByte_LowWord:
                    {
                        UInt64 Result;
                        Result = Buffer[Pos + 6]; Result <<= 8;
                        Result |= Buffer[Pos + 7]; Result <<= 8;
                        Result |= Buffer[Pos + 4]; Result <<= 8;
                        Result |= Buffer[Pos + 5]; Result <<= 8;
                        Result |= Buffer[Pos + 2]; Result <<= 8;
                        Result |= Buffer[Pos + 3]; Result <<= 8;
                        Result |= Buffer[Pos + 0]; Result <<= 8;
                        Result |= Buffer[Pos + 1];
                        return Result;
                    }
                // LowByte_HighWord FEHG
                case ByteOrder.LowByte_HighWord:
                    {
                        UInt64 Result;
                        Result = Buffer[Pos + 1]; Result <<= 8;
                        Result |= Buffer[Pos + 0]; Result <<= 8;
                        Result |= Buffer[Pos + 3]; Result <<= 8;
                        Result |= Buffer[Pos + 2]; Result <<= 8;
                        Result |= Buffer[Pos + 5]; Result <<= 8;
                        Result |= Buffer[Pos + 4]; Result <<= 8;
                        Result |= Buffer[Pos + 7]; Result <<= 8;
                        Result |= Buffer[Pos + 6];
                        return Result;
                    }
                // HGFE LowByte_LogWord
                case ByteOrder.LowByte_LowWord:
                    {
                        UInt64 Result;
                        Result = Buffer[Pos + 7]; Result <<= 8;
                        Result |= Buffer[Pos + 6]; Result <<= 8;
                        Result |= Buffer[Pos + 5]; Result <<= 8;
                        Result |= Buffer[Pos + 4]; Result <<= 8;
                        Result |= Buffer[Pos + 3]; Result <<= 8;
                        Result |= Buffer[Pos + 2]; Result <<= 8;
                        Result |= Buffer[Pos + 1]; Result <<= 8;
                        Result |= Buffer[Pos];
                        return Result;
                    }
                default:
                    return default;
            }
        }
        public static void SetULIntAt(byte[] Buffer, int Pos, UInt64 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            switch (byteOrder)
            {
                // HighByte_HighWord EFGH
                case ByteOrder.HighByte_HighWord:
                    Buffer[Pos + 7] = (byte)(Value & 0xFF);
                    Buffer[Pos + 6] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos + 0] = (byte)((Value >> 56) & 0xFF);
                    break;
                // GHEF HighByte_LowWord
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos + 1] = (byte)(Value & 0xFF);
                    Buffer[Pos + 0] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 7] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos + 6] = (byte)((Value >> 56) & 0xFF);
                    break;
                // LowByte_HighWord FEHG
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 6] = (byte)(Value & 0xFF);
                    Buffer[Pos + 7] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 0] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 56) & 0xFF);
                    break;
                // HGFE LowByte_LogWord
                case ByteOrder.LowByte_LowWord:
                    Buffer[Pos] = (byte)(Value & 0xFF);
                    Buffer[Pos + 1] = (byte)((Value >> 8) & 0xFF);
                    Buffer[Pos + 2] = (byte)((Value >> 16) & 0xFF);
                    Buffer[Pos + 3] = (byte)((Value >> 24) & 0xFF);
                    Buffer[Pos + 4] = (byte)((Value >> 32) & 0xFF);
                    Buffer[Pos + 5] = (byte)((Value >> 40) & 0xFF);
                    Buffer[Pos + 6] = (byte)((Value >> 48) & 0xFF);
                    Buffer[Pos + 7] = (byte)((Value >> 56) & 0xFF);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set 8 bit word (S7 Byte) 16#00..16#FF
        public static byte GetByteAt(byte[] Buffer, int Pos)
        {
            return Buffer[Pos];
        }
        public static void SetByteAt(byte[] Buffer, int Pos, byte Value)
        {
            Buffer[Pos] = Value;
        }
        #endregion

        #region Get/Set 16 bit word (S7 Word) 16#0000..16#FFFF
        public static UInt16 GetWordAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            return GetUIntAt(Buffer, Pos, byteOrder);
        }
        public static void SetWordAt(byte[] Buffer, int Pos, UInt16 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            SetUIntAt(Buffer, Pos, Value, byteOrder);
        }
        #endregion

        #region Get/Set 32 bit word (S7 DWord) 16#00000000..16#FFFFFFFF
        public static UInt32 GetDWordAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            return GetUDIntAt(Buffer, Pos, byteOrder);
        }
        public static void SetDWordAt(byte[] Buffer, int Pos, UInt32 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            SetUDIntAt(Buffer, Pos, Value, byteOrder);
        }
        #endregion

        #region Get/Set 64 bit word (S7 LWord) 16#0000000000000000..16#FFFFFFFFFFFFFFFF
        public static UInt64 GetLWordAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            return GetULIntAt(Buffer, Pos, byteOrder);
        }
        public static void SetLWordAt(byte[] Buffer, int Pos, UInt64 Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            SetULIntAt(Buffer, Pos, Value, byteOrder);
        }
        #endregion

        #region Get/Set 32 bit floating point number (S7 Real) (Range of Single)
        public static Single GetRealAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            UInt32 Value = GetUDIntAt(Buffer, Pos, byteOrder);
            byte[] bytes = BitConverter.GetBytes(Value);
            return BitConverter.ToSingle(bytes, 0);
        }
        public static void SetRealAt(byte[] Buffer, int Pos, Single Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            byte[] FloatArray = BitConverter.GetBytes(Value);
            switch (byteOrder)
            {
                case ByteOrder.HighByte_HighWord:
                    Buffer[Pos] = FloatArray[3];
                    Buffer[Pos + 1] = FloatArray[2];
                    Buffer[Pos + 2] = FloatArray[1];
                    Buffer[Pos + 3] = FloatArray[0];
                    break;
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 1] = FloatArray[3];
                    Buffer[Pos] = FloatArray[2];
                    Buffer[Pos + 3] = FloatArray[1];
                    Buffer[Pos + 2] = FloatArray[0];
                    break;
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos + 2] = FloatArray[3];
                    Buffer[Pos + 3] = FloatArray[2];
                    Buffer[Pos] = FloatArray[1];
                    Buffer[Pos + 1] = FloatArray[0];
                    break;
                case ByteOrder.LowByte_LowWord:
                    Buffer[Pos + 3] = FloatArray[3];
                    Buffer[Pos + 2] = FloatArray[2];
                    Buffer[Pos + 1] = FloatArray[1];
                    Buffer[Pos] = FloatArray[0];
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set 64 bit floating point number (S7 LReal) (Range of Double)
        public static Double GetLRealAt(byte[] Buffer, int Pos, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            UInt64 Value = GetULIntAt(Buffer, Pos, byteOrder);
            byte[] bytes = BitConverter.GetBytes(Value);
            return BitConverter.ToDouble(bytes, 0);
        }
        public static void SetLRealAt(byte[] Buffer, int Pos, Double Value, ByteOrder byteOrder = ByteOrder.HighByte_HighWord)
        {
            byte[] FloatArray = BitConverter.GetBytes(Value);
            switch (byteOrder)
            {
                // HighByte_HighWord EFGH
                case ByteOrder.HighByte_HighWord:
                    Buffer[Pos + 0] = FloatArray[7];
                    Buffer[Pos + 1] = FloatArray[6];
                    Buffer[Pos + 2] = FloatArray[5];
                    Buffer[Pos + 3] = FloatArray[4];
                    Buffer[Pos + 4] = FloatArray[3];
                    Buffer[Pos + 5] = FloatArray[2];
                    Buffer[Pos + 6] = FloatArray[1];
                    Buffer[Pos + 7] = FloatArray[0];
                    break;
                // GHEF HighByte_LowWord
                case ByteOrder.HighByte_LowWord:
                    Buffer[Pos + 6] = FloatArray[7];
                    Buffer[Pos + 7] = FloatArray[6];
                    Buffer[Pos + 4] = FloatArray[5];
                    Buffer[Pos + 5] = FloatArray[4];
                    Buffer[Pos + 2] = FloatArray[3];
                    Buffer[Pos + 3] = FloatArray[2];
                    Buffer[Pos + 0] = FloatArray[1];
                    Buffer[Pos + 1] = FloatArray[0];
                    break;
                // LowByte_HighWord FEHG
                case ByteOrder.LowByte_HighWord:
                    Buffer[Pos + 1] = FloatArray[7];
                    Buffer[Pos + 0] = FloatArray[6];
                    Buffer[Pos + 3] = FloatArray[5];
                    Buffer[Pos + 2] = FloatArray[4];
                    Buffer[Pos + 5] = FloatArray[3];
                    Buffer[Pos + 4] = FloatArray[2];
                    Buffer[Pos + 7] = FloatArray[1];
                    Buffer[Pos + 6] = FloatArray[0];
                    break;
                // HGFE LowByte_LogWord
                case ByteOrder.LowByte_LowWord:
                    Buffer[Pos + 7] = FloatArray[7];
                    Buffer[Pos + 6] = FloatArray[6];
                    Buffer[Pos + 5] = FloatArray[5];
                    Buffer[Pos + 4] = FloatArray[4];
                    Buffer[Pos + 3] = FloatArray[3];
                    Buffer[Pos + 2] = FloatArray[2];
                    Buffer[Pos + 1] = FloatArray[1];
                    Buffer[Pos] = FloatArray[0];
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Get/Set DateTime (S7 DATE_AND_TIME)
        public static DateTime GetDateTimeAt(byte[] Buffer, int Pos)
        {
            int Year, Month, Day, Hour, Min, Sec, MSec;

            Year = BCDtoByte(Buffer[Pos]);
            if (Year < 90)
                Year += 2000;
            else
                Year += 1900;

            Month = BCDtoByte(Buffer[Pos + 1]);
            Day = BCDtoByte(Buffer[Pos + 2]);
            Hour = BCDtoByte(Buffer[Pos + 3]);
            Min = BCDtoByte(Buffer[Pos + 4]);
            Sec = BCDtoByte(Buffer[Pos + 5]);
            MSec = (BCDtoByte(Buffer[Pos + 6]) * 10) + (BCDtoByte(Buffer[Pos + 7]) / 10);
            try
            {
                return new DateTime(Year, Month, Day, Hour, Min, Sec, MSec);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return new DateTime(0);
            }
        }
        public static void SetDateTimeAt(byte[] Buffer, int Pos, DateTime Value)
        {
            int Year = Value.Year;
            int Month = Value.Month;
            int Day = Value.Day;
            int Hour = Value.Hour;
            int Min = Value.Minute;
            int Sec = Value.Second;
            int Dow = (int)Value.DayOfWeek + 1;
            // MSecH = First two digits of miliseconds 
            int MsecH = Value.Millisecond / 10;
            // MSecL = Last digit of miliseconds
            int MsecL = Value.Millisecond % 10;
            if (Year > 1999)
                Year -= 2000;

            Buffer[Pos] = ByteToBCD(Year);
            Buffer[Pos + 1] = ByteToBCD(Month);
            Buffer[Pos + 2] = ByteToBCD(Day);
            Buffer[Pos + 3] = ByteToBCD(Hour);
            Buffer[Pos + 4] = ByteToBCD(Min);
            Buffer[Pos + 5] = ByteToBCD(Sec);
            Buffer[Pos + 6] = ByteToBCD(MsecH);
            Buffer[Pos + 7] = ByteToBCD(MsecL * 10 + Dow);
        }
        #endregion

        #region Get/Set DATE (S7 DATE) 
        public static DateTime GetDateAt(byte[] Buffer, int Pos)
        {
            try
            {
                return new DateTime(1990, 1, 1).AddDays(GetIntAt(Buffer, Pos));
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return new DateTime(0);
            }
        }
        public static void SetDateAt(byte[] Buffer, int Pos, DateTime Value)
        {
            SetIntAt(Buffer, Pos, (Int16)(Value - new DateTime(1990, 1, 1)).Days);
        }

        #endregion

        #region Get/Set TOD (S7 TIME_OF_DAY)
        public static DateTime GetTODAt(byte[] Buffer, int Pos)
        {
            try
            {
                return new DateTime(0).AddMilliseconds(ByteHelper.GetDIntAt(Buffer, Pos));
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return new DateTime(0);
            }
        }
        public static void SetTODAt(byte[] Buffer, int Pos, DateTime Value)
        {
            TimeSpan Time = Value.TimeOfDay;
            SetDIntAt(Buffer, Pos, (Int32)Math.Round(Time.TotalMilliseconds));
        }
        #endregion

        #region Get/Set LTOD (S7 1500 LONG TIME_OF_DAY)
        public static DateTime GetLTODAt(byte[] Buffer, int Pos)
        {
            // .NET Tick = 100 ns, S71500 Tick = 1 ns
            try
            {
                return new DateTime(Math.Abs(GetLIntAt(Buffer, Pos) / 100));
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return new DateTime(0);
            }
        }
        public static void SetLTODAt(byte[] Buffer, int Pos, DateTime Value)
        {
            TimeSpan Time = Value.TimeOfDay;
            SetLIntAt(Buffer, Pos, (Int64)Time.Ticks * 100);
        }
        #endregion

        #region GET/SET LDT (S7 1500 Long Date and Time)
        public static DateTime GetLDTAt(byte[] Buffer, int Pos)
        {
            try
            {
                return new DateTime((GetLIntAt(Buffer, Pos) / 100) + bias);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return new DateTime(0);
            }
        }
        public static void SetLDTAt(byte[] Buffer, int Pos, DateTime Value)
        {
            SetLIntAt(Buffer, Pos, (Value.Ticks - bias) * 100);
        }
        #endregion

        #region Get/Set DTL (S71200/1500 Date and Time)
        // Thanks to Johan Cardoen for GetDTLAt
        public static DateTime GetDTLAt(byte[] Buffer, int Pos)
        {
            int Year, Month, Day, Hour, Min, Sec, MSec;

            Year = Buffer[Pos] * 256 + Buffer[Pos + 1];
            Month = Buffer[Pos + 2];
            Day = Buffer[Pos + 3];
            Hour = Buffer[Pos + 5];
            Min = Buffer[Pos + 6];
            Sec = Buffer[Pos + 7];
            MSec = (int)GetUDIntAt(Buffer, Pos + 8) / 1000000;

            try
            {
                return new DateTime(Year, Month, Day, Hour, Min, Sec, MSec);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return new DateTime(0);
            }
        }
        public static void SetDTLAt(byte[] Buffer, int Pos, DateTime Value)
        {
            short Year = (short)Value.Year;
            byte Month = (byte)Value.Month;
            byte Day = (byte)Value.Day;
            byte Hour = (byte)Value.Hour;
            byte Min = (byte)Value.Minute;
            byte Sec = (byte)Value.Second;
            byte Dow = (byte)(Value.DayOfWeek + 1);

            Int32 NanoSecs = Value.Millisecond * 1000000;

            var bytes_short = BitConverter.GetBytes(Year);

            Buffer[Pos] = bytes_short[1];
            Buffer[Pos + 1] = bytes_short[0];
            Buffer[Pos + 2] = Month;
            Buffer[Pos + 3] = Day;
            Buffer[Pos + 4] = Dow;
            Buffer[Pos + 5] = Hour;
            Buffer[Pos + 6] = Min;
            Buffer[Pos + 7] = Sec;
            SetDIntAt(Buffer, Pos + 8, NanoSecs);
        }

        #endregion

        #region Get/Set String (S7 String)
        // Thanks to Pablo Agirre 
        public static string GetStringAt(byte[] Buffer, int Pos)
        {
            int size = (int)Buffer[Pos + 1];
            return Encoding.ASCII.GetString(Buffer, Pos + 2, size);
        }

        public static void SetStringAt(byte[] Buffer, int Pos, int MaxLen, string Value)
        {
            int size = Value.Length;
            Buffer[Pos] = (byte)MaxLen;
            Buffer[Pos + 1] = (byte)size;
            Encoding.ASCII.GetBytes(Value, 0, size, Buffer, Pos + 2);
        }
        #endregion

        #region Get/Set Array of char (S7 ARRAY OF CHARS)
        public static string GetCharsAt(byte[] Buffer, int Pos, int Size)
        {
            return Encoding.ASCII.GetString(Buffer, Pos, Size);
        }
        public static void SetCharsAt(byte[] Buffer, int Pos, string Value)
        {
            int MaxLen = Buffer.Length - Pos;
            // Truncs the string if there's no room enough        
            if (MaxLen > Value.Length) MaxLen = Value.Length;
            Encoding.ASCII.GetBytes(Value, 0, MaxLen, Buffer, Pos);
        }
        #endregion

        #region Get/Set Counter
        public static int GetCounter(ushort Value)
        {
            return BCDtoByte((byte)Value) * 100 + BCDtoByte((byte)(Value >> 8));
        }

        public static int GetCounterAt(ushort[] Buffer, int Index)
        {
            return GetCounter(Buffer[Index]);
        }

        public static ushort ToCounter(int Value)
        {
            return (ushort)(ByteToBCD(Value / 100) + (ByteToBCD(Value % 100) << 8));
        }

        public static void SetCounterAt(ushort[] Buffer, int Pos, int Value)
        {
            Buffer[Pos] = ToCounter(Value);
        }
        #endregion

        #endregion [Help Functions]
    }
}
