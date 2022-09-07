using ClosedXML.Excel;
using CommonControls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MayCatServer
{
    public class Repository
    {
        public static Repository Instance { get; } = new Repository();
        public string ConnectionString { get => Properties.Settings.Default["constr"].ToString(); }
        public string Ca { get; set; } = "1";

        public ThongTinCa GetThongTinCa()
        {
            ThongTinCa thongTinCa = new ThongTinCa() { ThoiGian = DateTime.Now, Ca = Convert.ToInt32(Ca) };
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"select * from ca where ThoiGian = '{DateTime.Now.ToString("yyyy-MM-dd")}' and Ca = '{Ca}'";
                        using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                
                                if (DateTime.TryParse(dt.Rows[0]["ThoiGian"]?.ToString(), out DateTime time))
                                    thongTinCa.ThoiGian = time;
                                if (int.TryParse(dt.Rows[0]["Ca"]?.ToString(), out int ca))
                                    thongTinCa.Ca = ca;
                                if (int.TryParse(dt.Rows[0]["SoMet"]?.ToString(), out int SoMet))
                                    thongTinCa.SoMet = SoMet;
                                if (int.TryParse(dt.Rows[0]["SoMetDat"]?.ToString(), out int SoMetDat))
                                    thongTinCa.SoMetDat = SoMetDat;
                                if (int.TryParse(dt.Rows[0]["SoMetLoi"]?.ToString(), out int SoMetLoi))
                                    thongTinCa.SoMetLoi = SoMetLoi;
                                if (int.TryParse(dt.Rows[0]["TocDoTB"]?.ToString(), out int TocDoTB))
                                    thongTinCa.TocDoTB = TocDoTB;
                                if (TimeSpan.TryParse(dt.Rows[0]["Chay"]?.ToString(), out TimeSpan Chay))
                                    thongTinCa.Chay = Chay;
                                if (TimeSpan.TryParse(dt.Rows[0]["Dung"]?.ToString(), out TimeSpan Dung))
                                    thongTinCa.Dung = Dung;
                                if (int.TryParse(dt.Rows[0]["SoDung"]?.ToString(), out int SoDung))
                                    thongTinCa.SoDung = SoDung;
                                if (int.TryParse(dt.Rows[0]["M2Dat"]?.ToString(), out int M2Dat))
                                    thongTinCa.M2Dat = M2Dat;
                                if (int.TryParse(dt.Rows[0]["M2Loi"]?.ToString(), out int M2Loi))
                                    thongTinCa.M2Loi = M2Loi;
                                if (int.TryParse(dt.Rows[0]["Count"]?.ToString(), out int Count))
                                    thongTinCa.Count = Count;
                            }
                            else
                            {
                                cmd.CommandText = $"insert into Ca (ThoiGian, Ca) values ('{DateTime.Now.ToString("yyyy-MM-dd")}', '{Ca}')";
                                int result = cmd.ExecuteNonQuery();
                                if (result > 0)
                                    return thongTinCa;
                            }
                        }
                    }
                }
            }
            catch { }
            return thongTinCa;
        }

        public int UpdateThongTinCa(ThongTinCa ca)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        string chay = $"{ca.Chay.Hours}:{ca.Chay.Minutes.ToString("00")}:{ca.Chay.Seconds.ToString("00")}";
                        string dung = $"{ca.Dung.Hours}:{ca.Dung.Minutes.ToString("00")}:{ca.Dung.Seconds.ToString("00")}";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"update ca set SoMet = '{ca.SoMet}', SoMetDat = '{ca.SoMetDat}', " +
                            $"SoMetLoi = '{ca.SoMetLoi}', TocDoTB = '{ca.TocDoTB}', " +
                            $"SoDung = '{ca.SoDung}', M2Dat = '{ca.M2Dat}', M2Loi = '{ca.M2Loi}', Count = '{ca.Count}', " +
                            $"Chay = '{chay}', Dung = '{dung}' where ThoiGian = '{ca.ThoiGian.ToString("yyyy-MM-dd")}' and Ca = '{ca.Ca}'";
                        int result = cmd.ExecuteNonQuery();

                        if (result == 0)
                        {
                            cmd.CommandText = $"insert into Ca (ThoiGian, Ca, SoMet, SoMetDat, SoMetLoi, TocDoTB, SoDung, M2Dat, M2Loi, Count, Chay, Dung) values " +
                                $"('{DateTime.Now.ToString("yyyy-MM-dd")}', '{Ca}', '{ca.SoMet}',  '{ca.SoMetDat}',  '{ca.SoMetLoi}'" +
                                $",  '{ca.TocDoTB}',  '{ca.SoDung}',  '{ca.M2Dat}',  '{ca.M2Loi}',  '{ca.Count}'" +
                                $",  '{chay}',  '{dung}')";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return 0;
            }
            catch { return 0; }
        }

        public List<DonHang> GetDonHangDataChay(string query)
        {
            var donHangs = new List<DonHang>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;

                        using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adp.Fill(dt);
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                DonHang dh = new DonHang();
                                if (long.TryParse(dtRow["Id"]?.ToString(), out long id))
                                    dh.Id = id;
                                if (long.TryParse(dtRow["STT"]?.ToString(), out long stt))
                                    dh.STT = stt;
                                if (DateTime.TryParse(dtRow["NgayTao"]?.ToString(), out DateTime ngayTao))
                                    dh.NgayTao = ngayTao;
                                dh.Ma = dtRow["Ma"]?.ToString();
                                dh.Song = dtRow["Song"]?.ToString();
                                if (int.TryParse(dtRow["Kho"]?.ToString(), out int kho))
                                    dh.Kho = kho;
                                if (int.TryParse(dtRow["Dai"]?.ToString(), out int dai))
                                    dh.Dai = dai;
                                if (int.TryParse(dtRow["SL"]?.ToString(), out int sl))
                                    dh.SL = sl;
                                if (int.TryParse(dtRow["SLDat"]?.ToString(), out int slDat))
                                    dh.SLDat = slDat;
                                if (int.TryParse(dtRow["SLLoi"]?.ToString(), out int slLoi))
                                    dh.SLLoi = slLoi;
                                if (TimeSpan.TryParse(dtRow["TGChay"]?.ToString(), out TimeSpan chay))
                                    dh.Chay = chay;
                                if (TimeSpan.TryParse(dtRow["TGDung"]?.ToString(), out TimeSpan dung))
                                    dh.Dung = dung;
                                if (DateTime.TryParse(dtRow["TGBatDau"]?.ToString(), out DateTime tgBatDau))
                                    dh.TGBatDau = tgBatDau;
                                if (DateTime.TryParse(dtRow["TGKetThuc"]?.ToString(), out DateTime tgKetThuc))
                                    dh.TGKetThuc = tgKetThuc;
                                if (int.TryParse(dtRow["SoLanDung"]?.ToString(), out int sld))
                                    dh.SoDung = sld;
                                if (int.TryParse(dtRow["Pallet"]?.ToString(), out int pallet))
                                    dh.Pallet = pallet;
                                if (int.TryParse(dtRow["Xa"]?.ToString(), out int xa))
                                    dh.Xa = xa;
                                if (int.TryParse(dtRow["Rong"]?.ToString(), out int rong))
                                    dh.Rong = rong;
                                if (int.TryParse(dtRow["Canh"]?.ToString(), out int canh))
                                    dh.Canh = canh;
                                if (int.TryParse(dtRow["Cao"]?.ToString(), out int cao))
                                    dh.Cao = cao;
                                if (int.TryParse(dtRow["Lang"]?.ToString(), out int lang))
                                    dh.Lang = lang;
                                if (int.TryParse(dtRow["Ca"]?.ToString(), out int ca))
                                    dh.Ca = ca;
                                dh.GhiChu = dtRow["GhiChu"]?.ToString();
                                dh.GiaySongE = dtRow["GiaySongE"]?.ToString();
                                dh.GiaySongB = dtRow["GiaySongB"]?.ToString();
                                dh.GiaySongC = dtRow["GiaySongC"]?.ToString();
                                dh.GiayMatE = dtRow["GiayMatE"]?.ToString();
                                dh.GiayMatB = dtRow["GiayMatB"]?.ToString();
                                dh.GiayMatC = dtRow["GiayMatC"]?.ToString();
                                dh.Men = dtRow["GiayMen"]?.ToString();
                                donHangs.Add(dh);
                            }
                        }
                        return new List<DonHang>(donHangs.OrderBy(x => x.STT));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return donHangs;
        }

        public List<DonHang> GetDonHangs(string query = "")
        {
            var donHangs = new List<DonHang>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        if (query == "")
                            cmd.CommandText = $"select * from dhdangchay where Ca = '{Ca}'";
                        else
                            cmd.CommandText = query;
                        using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adp.Fill(dt);
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                DonHang dh = new DonHang();

                                if (dt.Columns.Contains("Line"))
                                {
                                    if (int.TryParse(dtRow["Line"]?.ToString(), out int line))
                                    {
                                        if (line >= 0)
                                            dh.Line = line;
                                    }
                                }

                                if (dt.Columns.Contains("GiayDai"))
                                {
                                    if (int.TryParse(dtRow["GiayDai"]?.ToString(), out int giayDai))
                                    {
                                        if (giayDai >= 0)
                                            dh.GiayDai = giayDai;
                                    }
                                }
                                if (dt.Columns.Contains("GiayRong"))
                                {
                                    if (int.TryParse(dtRow["GiayRong"]?.ToString(), out int GiayRong))
                                    {
                                        if (GiayRong >= 0)
                                            dh.GiayRong = GiayRong;
                                    }
                                }

                                if (dt.Columns.Contains("GiayCao"))
                                {
                                    if (int.TryParse(dtRow["GiayCao"]?.ToString(), out int GiayCao))
                                    {
                                        if (GiayCao >= 0)
                                            dh.GiayCao = GiayCao;
                                    }
                                }

                                dh.KhachHang = dtRow["KhachHang"].ToString();
                                dh.TenDonHang = dtRow["TenDonHang"].ToString();
                                dh.PO = dtRow["PO"].ToString();
                                dh.MayIn = dtRow["MayIn"].ToString();
                                dh.Chap_Be = dtRow["Chap_Be"].ToString();
                                dh.Ghim_Dan = dtRow["Ghim_Dan"].ToString();

                                if (dt.Columns.Contains("MayXa"))
                                {
                                    if (int.TryParse(dtRow["MayXa"]?.ToString(), out int mayXa))
                                    {
                                        if (mayXa > 0 && mayXa <= 2)
                                            dh.MayXa = mayXa;
                                        else
                                            dh.MayXa = 1;
                                    }
                                }

                                if (long.TryParse(dtRow["Id"]?.ToString(), out long id))
                                    dh.Id = id;
                                if (long.TryParse(dtRow["STT"]?.ToString(), out long stt))
                                    dh.STT = stt;
                                if (DateTime.TryParse(dtRow["NgayTao"]?.ToString(), out DateTime ngayTao))
                                    dh.NgayTao = ngayTao;
                                dh.Ma = dtRow["Ma"]?.ToString();
                                dh.Song = dtRow["Song"]?.ToString();
                                if (int.TryParse(dtRow["Kho"]?.ToString(), out int kho))
                                    dh.Kho = kho;
                                if (int.TryParse(dtRow["Dai"]?.ToString(), out int dai))
                                    dh.Dai = dai;
                                if (int.TryParse(dtRow["SL"]?.ToString(), out int sl))
                                    dh.SL = sl;
                                if (int.TryParse(dtRow["SLDat"]?.ToString(), out int slDat))
                                    dh.SLDat = slDat;
                                if (int.TryParse(dtRow["SLLoi"]?.ToString(), out int slLoi))
                                    dh.SLLoi = slLoi;
                                if (TimeSpan.TryParse(dtRow["TGChay"]?.ToString(), out TimeSpan chay))
                                    dh.Chay = chay;
                                if (TimeSpan.TryParse(dtRow["TGDung"]?.ToString(), out TimeSpan dung))
                                    dh.Dung = dung;
                                if (DateTime.TryParse(dtRow["TGBatDau"]?.ToString(), out DateTime tgBatDau))
                                    dh.TGBatDau = tgBatDau;
                                if (DateTime.TryParse(dtRow["TGKetThuc"]?.ToString(), out DateTime tgKetThuc))
                                    dh.TGKetThuc = tgKetThuc;
                                if (int.TryParse(dtRow["SoLanDung"]?.ToString(), out int sld))
                                    dh.SoDung = sld;
                                if (int.TryParse(dtRow["Pallet"]?.ToString(), out int pallet))
                                    dh.Pallet = pallet;
                                if (int.TryParse(dtRow["Xa"]?.ToString(), out int xa))
                                    dh.Xa = xa;
                                if (int.TryParse(dtRow["Rong"]?.ToString(), out int rong))
                                    dh.Rong = rong;
                                if (int.TryParse(dtRow["Canh"]?.ToString(), out int canh))
                                    dh.Canh = canh;
                                if (int.TryParse(dtRow["Cao"]?.ToString(), out int cao))
                                    dh.Cao = cao;
                                if (int.TryParse(dtRow["Lang"]?.ToString(), out int lang))
                                    dh.Lang = lang;
                                if (int.TryParse(dtRow["Ca"]?.ToString(), out int ca))
                                    dh.Ca = ca;
                                dh.GhiChu = dtRow["GhiChu"]?.ToString();
                                dh.GiaySongE = dtRow["GiaySongE"]?.ToString();
                                dh.GiaySongB = dtRow["GiaySongB"]?.ToString();
                                dh.GiaySongC = dtRow["GiaySongC"]?.ToString();
                                dh.GiayMatE = dtRow["GiayMatE"]?.ToString();
                                dh.GiayMatB = dtRow["GiayMatB"]?.ToString();
                                dh.GiayMatC = dtRow["GiayMatC"]?.ToString();
                                dh.Men = dtRow["GiayMen"]?.ToString();

                                if (int.TryParse(dtRow["HoanTatCutter"]?.ToString(), out int htCutter))
                                    dh.HoanTatCutter = htCutter;
                                if (int.TryParse(dtRow["HoanTatMayMen"]?.ToString(), out int htMen))
                                    dh.HoanTatMayMen = htMen;
                                if (int.TryParse(dtRow["HoanTatSongB"]?.ToString(), out int htB))
                                    dh.HoanTatSongB = htB;
                                if (int.TryParse(dtRow["HoanTatGiaySongB"]?.ToString(), out int htsB))
                                    dh.HoanTatGiaySongB = htsB;
                                if (int.TryParse(dtRow["HoanTatGiayMatB"]?.ToString(), out int htmB))
                                    dh.HoanTatGiayMatB = htmB;
                                if (int.TryParse(dtRow["HoanTatSongC"]?.ToString(), out int htC))
                                    dh.HoanTatSongC = htC;
                                if (int.TryParse(dtRow["HoanTatGiaySongC"]?.ToString(), out int htsC))
                                    dh.HoanTatGiaySongC = htsC;
                                if (int.TryParse(dtRow["HoanTatGiayMatC"]?.ToString(), out int htmC))
                                    dh.HoanTatGiayMatC = htmC;
                                if (int.TryParse(dtRow["HoanTatSongE"]?.ToString(), out int htE))
                                    dh.HoanTatSongE = htE;
                                if (int.TryParse(dtRow["HoanTatGiaySongE"]?.ToString(), out int htsE))
                                    dh.HoanTatGiaySongE = htsE;
                                if (int.TryParse(dtRow["HoanTatGiayMatE"]?.ToString(), out int htmE))
                                    dh.HoanTatGiayMatE = htmE;

                                if (int.TryParse(dtRow["HoanTatSpliter"]?.ToString(), out int htSpliter))
                                    dh.HoanTatSpliter = htSpliter;
                                donHangs.Add(dh);
                            }
                        }
                        return new List<DonHang>(donHangs.OrderBy(x => x.STT));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return donHangs;
        }

        internal int UpdateColumns(string v, object p)
        {
            throw new NotImplementedException();
        }

        public int LuuDonHangDaChay(DonHang dh)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        string tgChay = $"{dh.Chay.Hours}:{dh.Chay.Minutes.ToString("00")}:{dh.Chay.Seconds.ToString("00")}";
                        string tgDung = $"{dh.Dung.Hours}:{dh.Dung.Minutes.ToString("00")}:{dh.Dung.Seconds.ToString("00")}";
                        if (dh.TGKetThuc == DateTime.MinValue)
                        {
                            dh.TGKetThuc = DateTime.Now;
                        }
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"call proc_insert_donhangdachay({dh.STT}, '{dh.NgayTao.ToString("yyyy/MM/dd HH:mm:ss")}', {dh.SLDat}, {dh.SLLoi}, " +
                            $"'{tgChay}', '{tgDung}', '{dh.TGBatDau.ToString("yyyy/MM/dd HH:mm:ss")}', " +
                            $"'{dh.TGKetThuc.ToString("yyyy/MM/dd HH:mm:ss")}', '{dh.Ma}', '{dh.Song}', '{dh.Kho}', '{dh.Men}', " +
                            $"'{dh.GiaySongE}', '{dh.GiayMatE}', '{dh.GiaySongB}', '{dh.GiayMatB}', '{dh.GiaySongC}', '{dh.GiayMatC}', " +
                            $"'{dh.Dai}', '{dh.SL}', '{dh.Pallet}', '{dh.Xa}', '{dh.Rong}', '{dh.Canh}', '{dh.Cao}', " +
                            $"'{dh.Lang}', '{Ca}', '{dh.GhiChu}', '{dh.SoDung}', '{dh.TocDoTB}');";
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateColumn(string table, string col, string value, string where = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = $"update {table} set {col} = '{value}' {where}";
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public int UpdateColumns(string table, string set)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = $"update {table} {set}";
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public void TinhTongChieuDaiGiay(List<DonHang> donHangs, DonHang dhBatDau, string giay, out long daiSong, out long daiMat)
        {
            daiSong = 0;
            daiMat = 0;
            if (dhBatDau != null)
            {
                var donHangLoc = donHangs.Where(x => x.STT >= dhBatDau.STT).OrderBy(x => x.STT).ToList();
                if (donHangLoc.Count > 0)
                {
                    if (giay == "e")
                    {
                        if (!string.IsNullOrWhiteSpace(dhBatDau.GiaySongE))
                        {
                            string giayBatDau = dhBatDau.Kho.ToString() + dhBatDau.GiaySongE;
                            for (int i = 0; i < donHangLoc.Count; i++)
                            {
                                string giayHienTai = donHangLoc[i].Kho.ToString() + donHangLoc[i].GiaySongE;
                                if (giayBatDau == giayHienTai)
                                    daiSong += (donHangLoc[i].Dai * donHangLoc[i].SL);
                                else
                                    break;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(dhBatDau.GiayMatE))
                        {
                            string giayBatDau = dhBatDau.Kho.ToString() + dhBatDau.GiayMatE;
                            for (int i = 0; i < donHangLoc.Count; i++)
                            {
                                string giayHienTai = donHangLoc[i].Kho.ToString() + donHangLoc[i].GiayMatE;
                                if (giayBatDau == giayHienTai)
                                    daiMat += (donHangLoc[i].Dai * donHangLoc[i].SL);
                                else
                                    break;
                            }
                        }
                    }
                    else if (giay == "b")
                    {
                        if (!string.IsNullOrWhiteSpace(dhBatDau.GiaySongB))
                        {
                            string giayBatDau = dhBatDau.Kho.ToString() + dhBatDau.GiaySongB;
                            for (int i = 0; i < donHangLoc.Count; i++)
                            {
                                string giayHienTai = donHangLoc[i].Kho.ToString() + donHangLoc[i].GiaySongB;
                                if (giayBatDau == giayHienTai)
                                    daiSong += (donHangLoc[i].Dai * donHangLoc[i].SL);
                                else
                                    break;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(dhBatDau.GiayMatB))
                        {
                            string giayBatDau = dhBatDau.Kho.ToString() + dhBatDau.GiayMatB;
                            for (int i = 0; i < donHangLoc.Count; i++)
                            {
                                string giayHienTai = donHangLoc[i].Kho.ToString() + donHangLoc[i].GiayMatB;
                                if (giayBatDau == giayHienTai)
                                    daiMat += (donHangLoc[i].Dai * donHangLoc[i].SL);
                                else
                                    break;
                            }
                        }
                    }
                    else if (giay == "c")
                    {
                        if (!string.IsNullOrWhiteSpace(dhBatDau.GiaySongC))
                        {
                            string giayBatDau = dhBatDau.Kho.ToString() + dhBatDau.GiaySongC;
                            for (int i = 0; i < donHangLoc.Count; i++)
                            {
                                string giayHienTai = donHangLoc[i].Kho.ToString() + donHangLoc[i].GiaySongC;
                                if (giayBatDau == giayHienTai)
                                    daiSong += (donHangLoc[i].Dai * donHangLoc[i].SL);
                                else
                                    break;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(dhBatDau.GiayMatC))
                        {
                            string giayBatDau = dhBatDau.Kho.ToString() + dhBatDau.GiayMatC;
                            for (int i = 0; i < donHangLoc.Count; i++)
                            {
                                string giayHienTai = donHangLoc[i].Kho.ToString() + donHangLoc[i].GiayMatC;
                                if (giayBatDau == giayHienTai)
                                    daiMat += (donHangLoc[i].Dai * donHangLoc[i].SL);
                                else
                                    break;
                            }
                        }
                    }
                    else if (giay == "m")
                    {
                        if (!string.IsNullOrWhiteSpace(dhBatDau.Men))
                        {
                            string giayBatDau = dhBatDau.Kho.ToString() + dhBatDau.Men;
                            for (int i = 0; i < donHangLoc.Count; i++)
                            {
                                string giayHienTai = donHangLoc[i].Kho.ToString() + donHangLoc[i].Men;
                                if (giayBatDau == giayHienTai)
                                    daiMat += (donHangLoc[i].Dai * donHangLoc[i].SL);
                                else
                                    break;
                            }
                        }

                    }
                }
            }
        }

        public CaiDat GetCaiDat()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from caidat";
                    using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                    {
                        CaiDat caiDat = new CaiDat();

                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            caiDat.DanCatTam = Convert.ToDouble(dt.Rows[0]["DanCatTam"]);
                            caiDat.DanMayMen = Convert.ToDouble(dt.Rows[0]["DanMayMen"]);
                            caiDat.DanMayXa = Convert.ToDouble(dt.Rows[0]["DanMayXa"]);
                            caiDat.SoMetBaoChuanBiGiaySongB = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuanBiGiaySongB"]);
                            caiDat.SoMetBaoChuanBiGiaySongC = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuanBiGiaySongC"]);
                            caiDat.SoMetBaoChuanBiGiaySongE = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuanBiGiaySongE"]);
                            caiDat.SoMetBaoChuanBiMen = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuanBiMen"]);
                            caiDat.SoMetBaoChuyenDonSongB = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuyenDonSongB"]);
                            caiDat.SoMetBaoChuyenDonSongC = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuyenDonSongC"]);
                            caiDat.SoMetBaoChuyenDonSongE = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuyenDonSongE"]);
                            caiDat.SoMetBaoChuyenDonMayMen = Convert.ToDouble(dt.Rows[0]["SoMetBaoChuyenDonMayMen"]);
                            caiDat.ChieuDaiToiThieuChoPhepSuaDon = Convert.ToDouble(dt.Rows[0]["ThoiGianTinhTocDoTrungBinh"]);
                        }

                        return caiDat;
                    }
                }
            }
        }

        public void ExportToExcel(List<DonHang> donHangs, string title, string time)
        {
            if (donHangs != null)
            {
                try
                {
                    Process[] process = Process.GetProcesses();
                    if (process != null)
                    {
                        foreach (var proc in process)
                        {
                            if (proc.ProcessName.ToLower().Contains("excel"))
                            {
                                if (proc.MainWindowTitle == "Report.xlsx - Excel")
                                {
                                    proc.CloseMainWindow();
                                }
                            }
                        }
                    }
                }
                catch { }

                using (var workbook = new XLWorkbook())
                {
                    string _Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    var sheet = workbook.Worksheets.Add("Sheet1");

                    List<string> columns = new List<string>()
                    {
                        "STT","Mã","Sóng","Khổ","Mền","Sóng 1","Mặt 1", "Sóng 2", "Mặt 2", "Sóng 3", "Mặt 3", "Dài", "Số lượng", "Tổng",
                        "Pallet",  "Xả", "Rộng", "Cánh", "Cao", "Lằng", "Ghi chú", "Đạt", "Lỗi", "M2 Đạt", "M2 Lỗi", "% Lỗi", "Lề",
                        "% Lề", "Tốc độ", "Chạy", "Dừng", "Lần", "Ngày Sản Xuất", "Ngày Kết Thúc",
                    };

                    // Set title
                    sheet.Cell(1, 1).Value = title;
                    sheet.Cell(1, 1).Style.Font.FontSize = 16;
                    sheet.Cell(1, 1).Style.Font.Bold = true;
                    sheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    sheet.Range(1, 1, 1, columns.Count).Merge();

                    // Set Time
                    sheet.Cell(2, 1).Value = time;
                    sheet.Cell(2, 1).Style.Font.FontSize = 14;
                    sheet.Cell(2, 1).Style.Font.Italic = true;
                    sheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    sheet.Range(2, 1, 2, columns.Count).Merge();

                    int row = 3;
                    for (int i = 1; i <= columns.Count; i++)
                    {
                        var cell = sheet.Cell(row, i);
                        cell.Value = columns[i - 1];
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Font.FontColor = XLColor.DarkBlue;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        if (i >= 0 && i <= 4)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                        }
                        if (i >= 5 && i <= 11)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }

                        if (i >= 12 && i <= 15)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.Yellow;
                        }

                        if (i >= 16 && i <= 20)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.Orange;
                        }
                        if (i == 21)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.Red;
                            cell.Style.Font.FontColor = XLColor.White;
                        }

                        if (i >= 22)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                        }
                    }
                    
                    for (int i = 0; i < donHangs.Count; i++)
                    {
                        row++;
                        XLColor defaultColor = XLColor.Black;
                        if (donHangs[i].TGKetThuc == DateTime.MinValue || donHangs[i].SLDat < donHangs[i].SL)
                            defaultColor = XLColor.Red;
                        SetCellValue(sheet, donHangs[i].STT, row, 1, i, defaultColor);
                        defaultColor = XLColor.Black;
                        SetCellValue(sheet, donHangs[i].Ma, row, 2, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Song, row, 3, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Kho, row, 4, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Men, row, 5, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].GiaySongE, row, 6, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].GiayMatE, row, 7, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].GiaySongB, row, 8, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].GiayMatB, row, 9, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].GiaySongC, row, 10, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].GiayMatC, row, 11, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Dai, row, 12, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].SL, row, 13, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Tong, row, 14, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Pallet, row, 15, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Xa, row, 16, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Rong, row, 17, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Canh, row, 18, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Cao, row, 19, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Lang, row, 20, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].GhiChu, row, 21, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].SLDat, row, 22, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].SLLoi, row, 23, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].M2Dat, row, 24, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].M2Loi, row, 25, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].PhanTramLoi, row, 26, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].Le, row, 27, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].PhanTramLe, row, 28, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].TocDoTB, row, 29, i, defaultColor);
                        SetCellValue(sheet, $"{donHangs[i].Chay.Hours}:{donHangs[i].Chay.Minutes.ToString("00")}:{donHangs[i].Chay.Seconds.ToString("00")}", row, 30, i, defaultColor);
                        SetCellValue(sheet, $"{donHangs[i].Dung.Hours}:{donHangs[i].Dung.Minutes.ToString("00")}:{donHangs[i].Dung.Seconds.ToString("00")}", row, 31, i, defaultColor);
                        SetCellValue(sheet, donHangs[i].SoDung, row, 32, i, defaultColor);

                        string ngaySX = donHangs[i].TGBatDau == DateTime.MinValue ? "" : donHangs[i].TGBatDau.ToString("dd/MM/yyyy HH:mm:ss");
                        SetCellValue(sheet, ngaySX, row, 33, i, defaultColor);

                        string ngayKetThuc = donHangs[i].TGKetThuc == DateTime.MinValue ? "" : donHangs[i].TGKetThuc.ToString("dd/MM/yyyy HH:mm:ss");
                        SetCellValue(sheet, ngaySX, row, 34, i, defaultColor);
                    }

                    for (int i = 1; i <= columns.Count; i++)
                    {
                        sheet.Column(i).AdjustToContents();
                        if (sheet.Column(i).Width < 5)
                            sheet.Column(i).Width = 5;
                    }

                    using (var file = System.IO.File.OpenWrite(_Path + "\\Report" + ".xlsx"))
                    {
                        workbook.SaveAs(file);
                    }

                    try
                    {
                        System.Diagnostics.Process.Start(_Path + "\\Report" + ".xlsx");
                    }
                    catch { }
                }
            }
        }

        private void SetCellValue(IXLWorksheet sheet, object value, int row, int column, int index, XLColor foreColor)
        {
            sheet.Cell(row, column).Style.Font.FontColor = foreColor;
            sheet.Cell(row, column).Style.Font.FontSize = 12;
            if (index % 2 == 1)
                sheet.Cell(row, column).Style.Fill.BackgroundColor = XLColor.LightGray;
            sheet.Cell(row, column).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            sheet.Cell(row, column).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, column).Value = value?.ToString();
        }

        public bool isSendData = false;
        Matrix _matrix = new Matrix();
        public void HienThiMatrix(DonHang dh, TrangThaiDonHang trangThaiDonHang)
        {
            try
            {
                if (trangThaiDonHang != null)
                {
                    // khi nạp chỉ thay đổi những giá trị cần thiết(hiện đã chọn mặc định đẹp nhất vì thế nên thay đổi nội dung thôi)
                    // Vùng 1 nếu muốn chuyển động thì cho thông số Area1ShowEffect =202 nhé.
                    _matrix.ip = "192.168.1.203";

                    //if (checkBox1.Checked == true)// CÓ THỂ XEM CHIỀU DÀI TEXT ĐỂ LÀM ĐIỀU KIỆN CHUYỂN ĐỘNG HAY ĐỨNG IM. TEXT.LENG
                    //{
                    //    _matrix.Area1ShowEffect = 202;
                    //}
                    //else
                    //{
                    //    _matrix.Area1ShowEffect = 0;
                    //}

                    // CHỈ THAY ĐỔI TEXT CỦA VÙNG 1,4,5,6,8,9,10,11,12 THÔI NHÉ.
                    // Title
                    _matrix.Area1Text = "BAO BÌ HỢP PHÁT";
                    //_matrix.Area1ShowEffect = 202; // Title chạy

                    // MT Số Mét tổng chạy được 
                    _matrix.Area6Text = trangThaiDonHang.SoMetDat2;

                    // Tốc độ
                    _matrix.Area8Text = trangThaiDonHang.TocDoTB2;
                    // Phần trăm đạt
                    _matrix.Area9Text = trangThaiDonHang.PhanTramLoi2;
                    // tg chạy
                    _matrix.Area10Text = trangThaiDonHang.Chay2;
                    // Số lần dừng
                    _matrix.Area11Text = trangThaiDonHang.SoDung2;
                    // tg dừng
                    _matrix.Area12Text = trangThaiDonHang.Dung2;
                    _matrix.Sendata(out int errorCode);
                    //if (!isSendData)
                    //{
                    //    _matrix.Sendata(out int errorCode);
                    //    isSendData = true;
                    //}
                    //else
                    //{
                    //    _matrix.SendRealTimeData((dh.ConLai).ToString("f0"));
                    //}
                    // NÊN DÙNG MỘT THREAD RIÊNG ĐỂ QUÉT LỆNH GỮI TỪ CHƯƠNG TRÌNH CHÍNH
                    // TRÁNH TRƯỜNG HỢP MẤT KN ĐẾN BẢNG LED MATRIX LÀM ĐƠ TOÀN HỆ THỐNG, VÌ KHI MẤT KẾT NỐI LỆNH GỮI BỊ ĐƠ TẦM 10 GIÂY
                    // CÓ THỂ DÙNG TRY CATH ĐỂ TỐI ƯU HỆ THỐNG NHÉ.
                }
                else
                {

                    // khi nạp chỉ thay đổi những giá trị cần thiết(hiện đã chọn mặc định đẹp nhất vì thế nên thay đổi nội dung thôi)
                    // Vùng 1 nếu muốn chuyển động thì cho thông số Area1ShowEffect =202 nhé.
                    _matrix.ip = "192.168.1.203";

                    //if (checkBox1.Checked == true)// CÓ THỂ XEM CHIỀU DÀI TEXT ĐỂ LÀM ĐIỀU KIỆN CHUYỂN ĐỘNG HAY ĐỨNG IM. TEXT.LENG
                    //{
                    //    _matrix.Area1ShowEffect = 202;
                    //}
                    //else
                    //{
                    //    _matrix.Area1ShowEffect = 0;
                    //}

                    // CHỈ THAY ĐỔI TEXT CỦA VÙNG 1,4,5,6,8,9,10,11,12 THÔI NHÉ.
                    // Title
                    _matrix.Area1Text = "BAO BÌ HỢP PHÁT";
                    //_matrix.Area1ShowEffect = 202; // Title chạy
                    // MT               
                    _matrix.Area6Text = "";
                    // Tốc độ           
                    _matrix.Area8Text = "";
                    // Phần trăm đạt    
                    _matrix.Area9Text = "";
                    // tg chạy          
                    _matrix.Area10Text ="";
                    // Số lần dừng      
                    _matrix.Area11Text ="";
                    // tg dừng          
                    _matrix.Area12Text = "";
                    _matrix.Sendata(out int errorCode);
                }

                if (CutterTags.Instance.TocDo != null)
                {
                    if (double.TryParse(CutterTags.Instance.TocDo.Value, out double tocDo))
                    {
                        // TocDo
                        _matrix.Area4Text = tocDo.ToString("f0");
                    }
                    else
                    {
                        _matrix.Area4Text = "";
                    }
                }

                if (CutterTags.Instance.DoiDon != null)
                {
                    if (double.TryParse(CutterTags.Instance.DoiDon.Value, out double doiDon))
                    {
                        // CL
                        _matrix.Area5Text = (doiDon / 1000.0f).ToString("f0");
                    }
                    else
                    {
                        _matrix.Area5Text = "";
                    }
                }
            }
            catch { }
        }
    }


}
