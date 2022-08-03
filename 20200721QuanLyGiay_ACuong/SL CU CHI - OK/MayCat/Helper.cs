using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace MayCat
{
    public static class Helper
    {
        static Helper()
        {
            try
            {
                if (File.Exists("KheHoLang.json"))
                {
                    ObservableCollection<KheHoLang> result = JsonConvert.DeserializeObject<ObservableCollection<KheHoLang>>(File.ReadAllText("KheHoLang.json"));
                    if (result != null)
                    {
                        DanhSachKheHoLang = result;
                    }
                    else
                    {
                        DanhSachKheHoLang = new ObservableCollection<KheHoLang>();
                    }
                }
                else
                {
                    DanhSachKheHoLang = new ObservableCollection<KheHoLang>();
                    File.WriteAllText("KheHoLang.json", JsonConvert.SerializeObject(DanhSachKheHoLang));
                }

                if (File.Exists("DonHangTay.json"))
                {
                    NotifyCollection<DonHang> result = JsonConvert.DeserializeObject<NotifyCollection<DonHang>>(File.ReadAllText("DonHangTay.json"));
                    if (result != null)
                    {
                        DonHangTay = result;
                    }
                    else
                    {
                        DonHangTay = new NotifyCollection<DonHang>();
                    }
                }
                else
                {
                    DonHangTay = new NotifyCollection<DonHang>();
                    File.WriteAllText("DonHangTay.json", JsonConvert.SerializeObject(DonHangTay));
                }
            }
            catch { }

            DonHangLink = new NotifyCollection<DonHang>();

            DonHangTay.CollectionChanged += DonHangTay_CollectionChanged;
        }

        private static void DonHangTay_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LuuDonHang(DonHangTay, "DonHangTay.json");
        }

        public static ObservableCollection<KheHoLang> DanhSachKheHoLang { get; set; }
        public static NotifyCollection<DonHang> DonHangTay { get; set; }
        public static NotifyCollection<DonHang> DonHangLink { get; set; }

        public static byte Id_May1 { get; set; }
        public static byte Id_May2 { get; set; }
        public static string IpAddress_May1 { get; set; }
        public static string IpAddress_May2 { get; set; }
        public static ModbusReader ModbusMay1 { get; set; }
        public static ModbusReader ModbusMay2 { get; set; }
        public static string ConfigRTU1 { get; set; }
        public static string ConfigRTU2 { get; set; }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static bool IsDWord(this string value)
        {
            if (uint.TryParse(value, out uint uintValue))
                return true;
            return false;
        }

        public static bool IsBool(this string value)
        {
            if (bool.TryParse(value, out bool _))
                return true;
            return false;
        }

        public static bool IsDWord(this string value, out uint uintValue)
        {
            if (uint.TryParse(value, out uintValue))
                return true;
            return false;
        }

        public static bool IsWord(this string value)
        {
            if (ushort.TryParse(value, out ushort uintValue))
                return true;
            return false;
        }

        public static bool IsShort(this string value)
        {
            if (short.TryParse(value, out short uintValue))
                return true;
            return false;
        }

        public static bool IsLong(this string value)
        {
            if (int.TryParse(value, out int uintValue))
                return true;
            return false;
        }

        public static byte[] GetBytes(Tag tag, object value)
        {
            if (tag.DataTypeBase.TryParseToByteArray(value, tag.Gain, tag.Offset, out byte[] result, tag.ByteOrder))
            {
                return result;
            }
            return null;
        }

        public static string NapDon(DonHang donHang, int viTri, int may)
        {
            try
            {
                byte[] writeBuffer = null;
                if (viTri == 2)
                    writeBuffer = new byte[22 * 4];
                else
                    writeBuffer = new byte[8 * 4];

                TagContainerBase tagContainer = may == 1 ? May1Tags.Instance : (TagContainerBase)May2Tags.Instance;

                Tag sttTag = viTri == 1 ? tagContainer.STT1 : tagContainer.STT2;
                Tag xaTag = viTri == 1 ? tagContainer.Xa1 : tagContainer.Xa2;
                Tag nap1Tag = viTri == 1 ? tagContainer.Nap11 : tagContainer.Nap21;
                Tag caoTag = viTri == 1 ? tagContainer.Cao1 : tagContainer.Cao2;
                Tag nap2Tag = viTri == 1 ? tagContainer.Nap21 : tagContainer.Nap22;
                Tag langtag = viTri == 1 ? tagContainer.Lang1 : tagContainer.Lang2;
                Tag songTag = viTri == 1 ? tagContainer.Song1 : tagContainer.Song2;
                Tag kheHoTag = viTri == 1 ? tagContainer.KheHo1 : tagContainer.KheHo2;

                Tag dao1_svTag = viTri == 1 ? tagContainer.Dao1_SV : tagContainer.Dao1_SV;
                Tag dao2_svTag = viTri == 1 ? tagContainer.Dao2_SV : tagContainer.Dao2_SV;
                Tag dao3_svTag = viTri == 1 ? tagContainer.Dao3_SV : tagContainer.Dao3_SV;
                Tag dao4_svTag = viTri == 1 ? tagContainer.Dao4_SV : tagContainer.Dao4_SV;
                Tag dao5_svTag = viTri == 1 ? tagContainer.Dao5_SV : tagContainer.Dao5_SV;
                Tag hut_svTag = viTri == 1 ? tagContainer.Hut_SV : tagContainer.Hut_SV;
                Tag lang1_svTag = viTri == 1 ? tagContainer.Lang1_SV : tagContainer.Lang1_SV;
                Tag lang2_svTag = viTri == 1 ? tagContainer.Lang2_SV : tagContainer.Lang2_SV;
                Tag lang3_svTag = viTri == 1 ? tagContainer.Lang3_SV : tagContainer.Lang3_SV;
                Tag lang4_svTag = viTri == 1 ? tagContainer.Lang4_SV : tagContainer.Lang4_SV;
                Tag lang5_svTag = viTri == 1 ? tagContainer.Lang5_SV : tagContainer.Lang5_SV;
                Tag lang6_svTag = viTri == 1 ? tagContainer.Lang6_SV : tagContainer.Lang6_SV;
                Tag lang7_svTag = viTri == 1 ? tagContainer.Lang7_SV : tagContainer.Lang7_SV;
                Tag lang8_svTag = viTri == 1 ? tagContainer.Lang8_SV : tagContainer.Lang8_SV;

                Tag dao1UTag = tagContainer.Dao1_U;
                Tag dao2UTag = tagContainer.Dao2_U;
                Tag dao3UTag = tagContainer.Dao3_U;
                Tag dao4UTag = tagContainer.Dao4_U;
                Tag dao5UTag = tagContainer.Dao5_U;

                Tag lang1UTag = tagContainer.Lang1_U;
                Tag lang2UTag = tagContainer.Lang2_U;
                Tag lang3UTag = tagContainer.Lang3_U;
                Tag lang4UTag = tagContainer.Lang4_U;
                Tag lang5UTag = tagContainer.Lang5_U;
                Tag lang6UTag = tagContainer.Lang6_U;
                Tag lang7UTag = tagContainer.Lang7_U;
                Tag lang8UTag = tagContainer.Lang8_U;

                Tag setting1Tag = tagContainer.Setting1;
                Tag setting2Tag = tagContainer.Setting2;
                Tag setting3Tag = tagContainer.Setting3;
                Tag setting4Tag = tagContainer.Setting4;
                Tag setting5Tag = tagContainer.Setting5;

                byte[] buffer = null;
                int index = 0;
                // STT
                buffer = GetBytes(sttTag, donHang.STT);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                // Xả
                buffer = GetBytes(xaTag, donHang.Xa);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                // Nắp 1
                buffer = GetBytes(nap1Tag, donHang.Nap1);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                // Cao
                buffer = GetBytes(caoTag, donHang.Cao);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                // Nắp 2
                buffer = GetBytes(nap2Tag, donHang.Nap2);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                // Lằng
                buffer = GetBytes(langtag, donHang.Lang);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                // Sóng
                buffer = GetBytes(songTag, 0);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                // Khe hở
                KheHoLang kheHoLang = DanhSachKheHoLang.FirstOrDefault(x => x.TenSong == donHang.Song);
                double kheHo = 0;
                if (kheHoLang != null)
                    kheHo = kheHoLang.KheHo;
                buffer = GetBytes(kheHoTag, kheHo);
                Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                index += buffer.Length;

                if (viTri == 2)
                {
                    int dao1_sv = 0, dao2_sv = 0, dao3_sv = 0, dao4_sv = 0, dao5_sv = 0, hut = 0, lang1_sv = 0, lang2_sv = 0, lang3_sv = 0, lang4_sv = 0, lang5_sv = 0, lang6_sv = 0, lang7_sv = 0, lang8_sv = 0;
                    int dao1_U = 50, dao2_U = 50, dao3_U = 50, dao4_U = 0, dao5_U = 0, Setting1 = 0, Setting2 = 2, Setting3 = 0, Setting4 = 0, Setting5 = 0;
                    int lang1_U = 0, lang2_U = 0, lang3_U =0, lang4_U = 0, lang5_U = 0, lang6_U = 0, lang7_U = 0, lang8_U = 0;
                    TinhToan.TinhToanGiaTri(donHang, ref dao1_sv, ref dao2_sv, ref dao3_sv, ref dao4_sv, ref dao5_sv, ref hut, ref lang1_sv, ref lang2_sv, ref lang3_sv, ref lang4_sv, ref lang5_sv, ref lang6_sv, ref lang7_sv, ref lang8_sv,
                        ref dao1_U, ref dao2_U, ref dao3_U, ref dao4_U, ref dao5_U, ref Setting1, ref Setting2, ref Setting3, ref Setting4, ref Setting5,
                        ref lang1_U, ref lang2_U, ref lang3_U, ref lang4_U, ref lang5_U, ref lang6_U, ref lang7_U, ref lang8_U, ref may);

                    // Dao 1
                    buffer = GetBytes(dao1_svTag, dao1_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index =36;
                    //index += buffer.Length;
                    
                    // Dao 2
                    buffer = GetBytes(dao2_svTag, dao2_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index =40;

                    // Dao 3
                    buffer = GetBytes(dao3_svTag, dao3_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index =44;

                    // Dao 4
                    buffer = GetBytes(dao4_svTag, dao4_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index =48;

                    // Dao 5
                    buffer = GetBytes(dao5_svTag, dao5_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index =52;

                    // Hút
                    buffer = GetBytes(hut_svTag, hut);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 56;

                    // Lằng 1
                    buffer = GetBytes(lang1_svTag, lang1_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 60;

                    // Lằng 2
                    buffer = GetBytes(lang2_svTag, lang2_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 64;

                    // Lằng 3
                    buffer = GetBytes(lang3_svTag, lang3_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 68;

                    // Lằng 4
                    buffer = GetBytes(lang4_svTag, lang4_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 72;

                    // Lằng 5
                    buffer = GetBytes(lang5_svTag, lang5_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 76;

                    // Lằng 6
                    buffer = GetBytes(lang6_svTag, lang6_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 80;

                    // Lằng 7
                    buffer = GetBytes(lang7_svTag, lang7_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 84;

                    // Lằng 8
                    buffer = GetBytes(lang8_svTag, lang8_sv);
                    Array.Copy(buffer, 0, writeBuffer, index, buffer.Length);
                    index = 88;

                    #region Ghi setting 1 -> 5
                    index = 0;
                    byte[] settingBuffer = new byte[5 * 4];

                    buffer = GetBytes(setting1Tag, Setting1);
                    Array.Copy(buffer, 0, settingBuffer, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(setting2Tag, Setting2);
                    Array.Copy(buffer, 0, settingBuffer, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(setting3Tag, Setting3);
                    Array.Copy(buffer, 0, settingBuffer, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(setting4Tag, Setting4);
                    Array.Copy(buffer, 0, settingBuffer, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(setting5Tag, Setting5);
                    Array.Copy(buffer, 0, settingBuffer, index, buffer.Length);
                    index += buffer.Length;

                    // Ghi xuống device
                    tagContainer.ModbusReader.WriteRegisters(setting1Tag.Address, settingBuffer);
                    #endregion

                    #region Ghi Dao/Lằng U
                    index = 0;
                    byte[] buf = new byte[13 * 4];

                    buffer = GetBytes(dao1UTag, dao1_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(dao2UTag, dao2_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(dao3UTag, dao3_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(dao4UTag, dao4_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(dao5UTag, dao5_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang1UTag, lang1_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang2UTag, lang2_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang3UTag, lang3_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang4UTag, lang4_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang5UTag, lang5_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang6UTag, lang6_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang7UTag, lang7_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    buffer = GetBytes(lang8UTag, lang8_U);
                    Array.Copy(buffer, 0, buf, index, buffer.Length);
                    index += buffer.Length;

                    // Ghi xuống device
                    tagContainer.ModbusReader.WriteRegisters(dao1UTag.Address, buf);
                    #endregion
                }

                bool result = tagContainer.ModbusReader.WriteRegisters(sttTag.Address, writeBuffer);

                if (result)
                    return string.Empty;
                else
                    return $"Nạp đơn {donHang.STT} vào máy {may}, vị trí {viTri} thất bại";
            }
            catch (Exception ex)
            {
                return $"Nạp đơn lỗi: {ex.ToString()}";
            }
        }

        public static DonHang LayDonHangKeTiep(Collection<DonHang> source, DonHang dhBatDau, int may)
        {
            var danhSach = source.OrderBy(x => x.STT).ToList();
            int indexBatDau = source.IndexOf(dhBatDau);
            if (indexBatDau < 0)
                return null;

            indexBatDau++;
            DonHang dhKe = null;

            if (may == 1)
            {
                if (!May1Tags.Instance.ChoPhepMayChay)
                    return null;

                for (int i = indexBatDau; i < danhSach.Count; i++)
                {
                    if (danhSach[i].Lang == 0 && danhSach[i].Xa == 0)
                        continue;

                    if (!May2Tags.Instance.ChoPhepMayChay)
                    {
                        dhKe = danhSach[i];
                        break;
                    }
                    if (i == indexBatDau)
                    {
                        if (danhSach[i].UuTien == "1")
                        {
                            dhKe = danhSach[i];
                            break;
                        }
                    }
                    else
                    {
                        if (danhSach[i].UuTien != "2")
                        {
                            dhKe = danhSach[i];
                            break;
                        }
                    }
                }

            }
            else if (may == 2)
            {
                if (!May2Tags.Instance.ChoPhepMayChay)
                    return null;

                for (int i = indexBatDau; i < danhSach.Count; i++)
                {
                    if (danhSach[i].Lang == 0 && danhSach[i].Xa == 0)
                        continue;

                    if (!May1Tags.Instance.ChoPhepMayChay)
                    {
                        dhKe = danhSach[i];
                        break;
                    }

                    if (i == indexBatDau)
                    {
                        if (danhSach[i].UuTien == "2")
                        {
                            dhKe = danhSach[i];
                            break;
                        }
                    }
                    else
                    {
                        if (danhSach[i].UuTien != "1")
                        {
                            dhKe = danhSach[i];
                            break;
                        }
                    }
                }
            }
            return dhKe;
        }

        public static bool LuuDonHang(NotifyCollection<DonHang> source, string name)
        {
            try
            {
                string saveContent = JsonConvert.SerializeObject(source, Formatting.Indented);
                File.WriteAllText(name, saveContent);
                return true;
            }
            catch { }
            return false;
        }
    }
}
