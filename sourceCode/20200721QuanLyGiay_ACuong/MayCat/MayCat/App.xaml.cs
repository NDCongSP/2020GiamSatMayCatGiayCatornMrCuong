using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MayCat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            List<PropertyInfo> properties1 = May1Tags.Instance.GetType().GetProperties().ToList();
            DataTable table1 = Helper.ConvertCSVtoDataTable("Slitter1.csv");
            foreach (DataRow row in table1.Rows)
            {
                string name = row[0].ToString();

                if (properties1.FirstOrDefault(x => x.Name == name) is PropertyInfo info)
                {
                    DataType dt = (DataType)Enum.Parse(typeof(DataType), row[2].ToString());
                    Tag tag = new Tag(row[0].ToString(), uint.Parse(row[1].ToString()), dt);
                    double gain = double.Parse(row[5].ToString());
                    double offset = double.Parse(row[6].ToString());
                    tag.Gain = gain;
                    tag.Offset = offset;
                    info.SetValue(May1Tags.Instance, tag);
                    May1Tags.Instance.TagSource.Add(tag);
                }
            }

            List<PropertyInfo> properties2 = May2Tags.Instance.GetType().GetProperties().ToList();
            DataTable table2 = Helper.ConvertCSVtoDataTable("Slitter2.csv");
            foreach (DataRow row in table2.Rows)
            {
                string name = row[0].ToString();

                if (properties2.FirstOrDefault(x => x.Name == name) is PropertyInfo info)
                {
                    DataType dt = (DataType)Enum.Parse(typeof(DataType), row[2].ToString());
                    Tag tag = new Tag(row[0].ToString(), uint.Parse(row[1].ToString()), dt);
                    double gain = double.Parse(row[5].ToString());
                    double offset = double.Parse(row[6].ToString());
                    tag.Gain = gain;
                    tag.Offset = offset;
                    info.SetValue(May2Tags.Instance, tag);
                    May2Tags.Instance.TagSource.Add(tag);
                }
            }

            Helper.Id_May1 = byte.Parse(MayCat.Properties.Settings.Default["Id_May1"].ToString());
            Helper.Id_May2 = byte.Parse(MayCat.Properties.Settings.Default["Id_May2"].ToString());
            Helper.IpAddress_May1 = MayCat.Properties.Settings.Default["IpAddress_May1"].ToString();
            Helper.IpAddress_May2 = MayCat.Properties.Settings.Default["IpAddress_May2"].ToString();
            Helper.ConfigRTU1 = MayCat.Properties.Settings.Default["CauHinhRTU1"].ToString();
            Helper.ConfigRTU2 = MayCat.Properties.Settings.Default["CauHinhRTU2"].ToString();

            May1Tags.Instance.ChoPhepMayChay = bool.Parse(MayCat.Properties.Settings.Default["ChoPhepChayMay1"].ToString());
            May2Tags.Instance.ChoPhepMayChay = bool.Parse(MayCat.Properties.Settings.Default["ChoPhepChayMay2"].ToString());

            string modbus = MayCat.Properties.Settings.Default["Modbus"].ToString().ToLower();
            ModbusReader modbus1 = null;
            ModbusReader modbus2 = null;
            // Use tcp
            if (modbus == "tcp")
            {
                modbus1 = new ModbusTCPReader(Helper.IpAddress_May1, Helper.Id_May1, May1Tags.Instance.TagSource, ByteOrder.CDAB);
                modbus2 = new ModbusTCPReader(Helper.IpAddress_May2, Helper.Id_May2, May2Tags.Instance.TagSource, ByteOrder.CDAB);
            }
            // Use rtu
            else
            {
                modbus1 = new ModbusRTUReader(Helper.ConfigRTU1, Helper.Id_May1, May1Tags.Instance.TagSource, ByteOrder.CDAB);
                modbus2 = new ModbusRTUReader(Helper.ConfigRTU2, Helper.Id_May2, May2Tags.Instance.TagSource, ByteOrder.CDAB);
            }

            May1Tags.Instance.ModbusReader = modbus1;
            May2Tags.Instance.ModbusReader = modbus2;

            Helper.ModbusMay1 = modbus1;
            Helper.ModbusMay1.Start();
            Helper.ModbusMay2 = modbus2;
            Helper.ModbusMay2.Start();

            May1Tags.Instance.RegisterEvent();
            May2Tags.Instance.RegisterEvent();

            base.OnStartup(e);
        }
    }
}
