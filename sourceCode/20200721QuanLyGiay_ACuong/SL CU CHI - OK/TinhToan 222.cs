using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public static class TinhToan
    {
        /// <summary>
        /// Hàm tính toán giá trị nạp xuống PLC
        /// </summary>
        /// <param name="dao1_sv"></param>
        /// <param name="dao2_sv"></param>
        /// <param name="dao3_sv"></param>
        /// <param name="dao4_sv"></param>
        /// <param name="dao5_sv"></param>
        /// <param name="hut"></param>
        /// <param name="lang1_sv"></param>
        /// <param name="lang2_sv"></param>
        /// <param name="lang3_sv"></param>
        /// <param name="lang4_sv"></param>
        /// <param name="lang5_sv"></param>
        /// <param name="lang6_sv"></param>
        /// <param name="lang7_sv"></param>
        /// <param name="lang8_sv"></param>
        public static void TinhToanGiaTri(DonHang donHang, ref int dao1_sv, ref int dao2_sv, ref int dao3_sv, ref int dao4_sv, ref int dao5_sv,
            ref int hut, ref int lang1_sv, ref int lang2_sv, ref int lang3_sv, ref int lang4_sv, ref int lang5_sv, ref int lang6_sv, ref int lang7_sv, ref int lang8_sv,
            ref int dao1_U, ref int dao2_U, ref int dao3_U, ref int dao4_U, ref int dao5_U, ref int Setting1, ref int Setting2, ref int Setting3, ref int Setting4, ref int Setting5,
            ref int lang1_U, ref int lang2_U, ref int lang3_U, ref int lang4_U, ref int lang5_U, ref int lang6_U, ref int lang7_U, ref int lang8_U, ref int may)
        {

            int M1_OffSet = (int)May1Tags.Instance.Setting9.Value;
            int M1_Dao_Dao = (int)May1Tags.Instance.Dao_Dao.Value;
            int M1_Dao1_Max = (int)May1Tags.Instance.Dao1_Max.Value;
            int M1_Dao2_Max = (int)May1Tags.Instance.Dao2_Max.Value;
            int M1_Dao1_PV = (int)May1Tags.Instance.Dao1_PV.Value;
            int M1_Dao2_PV = (int)May1Tags.Instance.Dao2_PV.Value;
            int M1_Dao4_PV = (int)May1Tags.Instance.Dao4_PV.Value;
            int M1_Dao5_PV = (int)May1Tags.Instance.Dao5_PV.Value;

            int M1_Lang_Lang = (int)May1Tags.Instance.Lang_Lang.Value;
            int M1_Lang1_PV = (int)May1Tags.Instance.Lang1_PV.Value;
            int M1_Lang2_PV = (int)May1Tags.Instance.Lang2_PV.Value;
            int M1_Lang3_PV = (int)May1Tags.Instance.Lang3_PV.Value;
            int M1_Lang4_PV = (int)May1Tags.Instance.Lang4_PV.Value;
            int M1_Lang5_PV = (int)May1Tags.Instance.Lang5_PV.Value;
            int M1_Lang6_PV = (int)May1Tags.Instance.Lang6_PV.Value;
            int M1_Lang7_PV = (int)May1Tags.Instance.Lang7_PV.Value;
            int M1_Lang8_PV = (int)May1Tags.Instance.Lang8_PV.Value;

            int M2_OffSet = (int)May2Tags.Instance.Setting9.Value;
            int M2_Dao_Dao = (int)May2Tags.Instance.Dao_Dao.Value;
            int M2_Dao1_Max = (int)May2Tags.Instance.Dao1_Max.Value;
            int M2_Dao2_Max = (int)May2Tags.Instance.Dao2_Max.Value;
            int M2_Dao1_PV = (int)May2Tags.Instance.Dao1_PV.Value;
            int M2_Dao2_PV = (int)May2Tags.Instance.Dao2_PV.Value;
            int M2_Dao4_PV = (int)May2Tags.Instance.Dao4_PV.Value;
            int M2_Dao5_PV = (int)May2Tags.Instance.Dao5_PV.Value;

            int M2_Lang_Lang = (int)May2Tags.Instance.Lang_Lang.Value;
            int M2_Lang1_PV = (int)May2Tags.Instance.Lang1_PV.Value;
            int M2_Lang2_PV = (int)May2Tags.Instance.Lang2_PV.Value;
            int M2_Lang3_PV = (int)May2Tags.Instance.Lang3_PV.Value;
            int M2_Lang4_PV = (int)May2Tags.Instance.Lang4_PV.Value;
            int M2_Lang5_PV = (int)May2Tags.Instance.Lang5_PV.Value;
            int M2_Lang6_PV = (int)May2Tags.Instance.Lang6_PV.Value;
            int M2_Lang7_PV = (int)May2Tags.Instance.Lang7_PV.Value;
            int M2_Lang8_PV = (int)May2Tags.Instance.Lang8_PV.Value;

            int Dao_Dao = 0, Dao1_Max = 0, Dao2_Max = 0, Dao4_Max = 0, Dao5_Max = 0, Dao1_PV = 0, Dao2_PV = 0, Dao4_PV = 0, Dao5_PV = 0, Dao3_Max = 0, Dao3_Min=0;
            int Lang_Lang = 0, Lang1_PV = 0, Lang2_PV = 0, Lang3_PV = 0, Lang4_PV = 0, Lang5_PV = 0, Lang6_PV = 0, Lang7_PV = 0, Lang8_PV = 0;
            
            if (may == 1)
            {
                Dao_Dao = M1_Dao_Dao;
                Dao1_Max = Dao5_Max = M1_Dao1_Max;
                Dao2_Max = Dao4_Max = M1_Dao2_Max;
                Dao3_Max = 200;
                Dao3_Min = -200;
                Dao1_PV = M1_Dao1_PV;
                Dao2_PV = M1_Dao2_PV;
                Dao4_PV = M1_Dao4_PV;
                Dao5_PV = M1_Dao5_PV;

                Lang_Lang = M1_Lang_Lang;
                Lang1_PV = M1_Lang1_PV;
                Lang2_PV = M1_Lang5_PV;
                Lang3_PV = M1_Lang2_PV;
                Lang4_PV = M1_Lang6_PV;
                Lang5_PV = M1_Lang3_PV;
                Lang6_PV = M1_Lang7_PV;
                Lang7_PV = M1_Lang4_PV;
                Lang8_PV = M1_Lang8_PV;


            }

            if (may == 2)
            {
                Dao_Dao = M2_Dao_Dao;
                Dao1_Max = Dao5_Max = M2_Dao1_Max;
                Dao2_Max = Dao4_Max = M2_Dao2_Max;
                Dao3_Max = 150;
                Dao3_Min = -150;
                Dao1_PV = M2_Dao1_PV;
                Dao2_PV = M2_Dao2_PV;
                Dao4_PV = M2_Dao4_PV;
                Dao5_PV = M2_Dao5_PV;

                Lang_Lang = M2_Lang_Lang;
                Lang1_PV = M2_Lang1_PV;
                Lang2_PV = M2_Lang2_PV;
                Lang3_PV = M2_Lang3_PV;
                Lang4_PV = M2_Lang4_PV;
                Lang5_PV = M2_Lang5_PV;
                Lang6_PV = M2_Lang6_PV;
                Lang7_PV = M2_Lang7_PV;
                Lang8_PV = M2_Lang8_PV;
            }


            int RongTotal = 0;
            int SoLangA = 0;
            int SoLangB = 0;
            int SoLangC = 0;
            int SoLangD = 0;
            int TongSoLang = 0;
            int LA1, LA2, LA3, LA4, LB1, LB2, LB3, LB4, LC1, LC2, LC3, LD1, LD2, LD3;
            LA1 = LA2 = LA3= LA4 = LB1 = LB2 = LB3 = LB4 = LC1 = LC2 = LC3 = LD1 = LD2 = LD3 = 0;
            
            int L1=0, L2=0, L3=0, L4=0, L5 = 0, L6 = 0, L7 =0, L8=0;


            int CaoA = donHang.Cao;
            int Nap1A = donHang.Nap1;
            int Nap2A = donHang.Nap2;
            int Nap3A = donHang.Nap3;
            int Nap4A = donHang.Nap4;
            int RongA = CaoA + Nap1A + Nap2A + Nap3A + Nap4A;

            int Rong = RongA;
            int Cao = CaoA;
            int Nap1 = Nap1A;
            int Nap2 = Nap2A;

            int CaoB = donHang.Cao_B;
            int Nap1B = donHang.Nap1_B;
            int Nap2B = donHang.Nap2_B;
            int Nap3B = donHang.Nap3_B;
            int Nap4B = donHang.Nap4_B;
            int RongB = CaoB + Nap1B + Nap2B + Nap3B + Nap4B;

            int CaoC = donHang.Cao_C;
            int Nap1C = donHang.Nap1_C;
            int Nap2C = donHang.Nap2_C;
            int Nap3C = donHang.Nap3_C;
            int RongC = donHang.RongC;

            int CaoD = donHang.Cao_D;
            int Nap1D = donHang.Nap1_D;
            int Nap2D = donHang.Nap2_D;
            int Nap3D = donHang.Nap3_D;
            int RongD = donHang.RongD;

            bool CoMoRong = donHang.CoMoRong;
            bool MoRongB = donHang.MoRongB;
            bool MoRongC = donHang.MoRongC;
            bool MoRongD = donHang.MoRongD;

            int Xa = donHang.Xa;
            int Lang = donHang.Lang;

            int Rongchia2 = Rong / 2;
            int Rongx3chia2 = (Rong * 3) / 2;
            int Cutline1 = 0, Cutline2 = 0, Cutline3 = 0, Cutline4 = 0, Cutline5 = 0;
            int[] Cutline = { Cutline1, Cutline2, Cutline3, Cutline4, Cutline5 };
            int[] D_Max = { Dao1_Max, Dao2_Max, Dao3_Max, Dao4_Max, Dao5_Max };
            int[] Dao = { dao1_sv, dao2_sv, dao3_sv, dao4_sv, dao5_sv };

            int i = 0;
            int j = 0;
            int m = 0;
            int n = 0;
            int l = 0;
            int k = 0;
            int SL_Am = 0;
            int SL_Duong = 0;

            dao1_U = 0;
            dao2_U = 0;
            dao3_U = 0;
            dao4_U = 0;
            dao5_U = 0;


            if (CoMoRong == false)          // Không Mở Rộng
            {
                if (Xa == 2)
                {
                    RongB = RongA;
                    CaoB = CaoA;
                    Nap1B = Nap1A;
                    Nap2B = Nap2A;
                    Nap3B = Nap3A;
                    Nap4B = Nap4A;
                }
                if (Xa == 3)
                {
                    RongB = RongC = RongA;
                    CaoB = CaoC = CaoA;
                    Nap1B = Nap1C = Nap1A;
                    Nap2B = Nap2C = Nap2A;
                    Nap3B = Nap3C = Nap3A;

                }
                if (Xa == 4)
                {
                    RongB = RongC = RongD = RongA;
                    CaoB = CaoC = CaoD = CaoA;
                    Nap1B = Nap1C = Nap1D = Nap1A;
                    Nap2B = Nap2C = Nap2D = Nap2A;
                    Nap3B = Nap3C = Nap3D = Nap3A;
                }

            }

            if (CoMoRong == true)          // Có Mở Rộng
            {

                if (MoRongB == true)
                    Xa = Xa + 1;
                if (MoRongC == true)
                    Xa = Xa + 1;
                if (MoRongD == true)
                    Xa = Xa + 1;

                if (RongA == 0)
                {
                    Xa = 0;
                    RongB = RongC = RongD = 0;
                    CaoB = CaoC = CaoD = 0;
                    Nap1B = Nap1C = Nap1D = 0;
                    Nap2B = Nap2C = Nap2D = 0;
                    Nap3B = Nap3C = Nap3D = 0;
                }
                if (CaoA == 0)
                    CaoB = CaoC = CaoD = 0;
                if (Nap1A == 0)
                    Nap1B = Nap1C = Nap1D = 0;
                if (Nap2A == 0)
                    Nap2B = Nap2C = Nap2D = 0;
                if (Nap3A == 0)
                    Nap3B = Nap3C = Nap3D = 0;

                if (RongB == 0)
                {
                    RongC = RongD = 0;
                    CaoC = CaoD = 0;
                    Nap1C = Nap1D = 0;
                    Nap2C = Nap2D = 0;
                    Nap3C = Nap3D = 0;
                }
                if (CaoB == 0)
                    CaoC = CaoD = 0;
                if (Nap1B == 0)
                    Nap1C = Nap1D = 0;
                if (Nap2B == 0)
                    Nap2C = Nap2D = 0;
                if (Nap3B == 0)
                    Nap3C = Nap3D = 0;


                if (RongC == 0)
                {
                    RongC = RongD = 0;
                    CaoC = CaoD = 0;
                    Nap1C = Nap1D = 0;
                    Nap2C = Nap2D = 0;
                    Nap3C = Nap3D = 0;
                }

                if (RongB == 0)
                    RongC = RongB;

            }

            if (Xa == 1)
                RongTotal = RongA;
            if (Xa == 2)
                RongTotal = RongA + RongB;
            if (Xa == 3)
                RongTotal = RongA + RongB + RongC;
            if (Xa == 4)
                RongTotal = RongA + RongB + RongC + RongD;

            if (Xa == 0)
            {
                dao1_U = 0;
                dao2_U = 0;
                dao3_U = 0;
                dao4_U = 0;
                dao5_U = 0;
            }
            // Xả = 1
            if (Xa == 1)
            {
                dao3_sv = 0;
                dao3_U = 0;

                // Xả 1 - Dao 1,2

                if (Dao2_Max < RongTotal / 2)                   //Dao2_Max > 0
                {
                    dao1_sv = -1 * RongTotal / 2;
                    dao1_U = 100;

                    if ((dao1_sv + Dao_Dao) > (Dao2_PV))
                        dao2_sv = dao1_sv + Dao_Dao;
                }

                if (Dao2_Max > RongTotal / 2)                   //Dao2_Max > 0
                {
                    dao2_sv = -1 * RongTotal / 2;
                    dao2_U = 100;

                    if ((dao2_sv - Dao_Dao) < (Dao1_PV))
                        dao1_sv = dao2_sv - Dao_Dao;

                }

                // Xả 1 - Dao 4,5
                if (Dao4_Max < RongTotal / 2)                   //Dao4_Max > 0
                {
                    dao5_sv = RongTotal / 2;
                    dao5_U = 100;
                    if ((dao5_sv - Dao_Dao) < (Dao4_PV))
                        dao4_sv = dao5_sv - Dao_Dao;
                }
                if (Dao4_Max > RongTotal / 2)                   //Dao4_Max > 0
                {
                    dao4_sv = RongTotal / 2;
                    dao4_U = 100;
                    if ((dao4_sv + Dao_Dao) > (Dao5_PV))
                        dao5_sv = dao4_sv + Dao_Dao;
                }
            }


        
            // Xả = 2
            if (Xa == 2)
            {

                // Xả 2 - Dao 1,2

                if (Dao2_Max < RongTotal/2)                   //Dao2_Max > 0
                {
                    dao1_sv = -1 * RongTotal / 2;
                    dao1_U = 100;

                    if ((dao1_sv + Dao_Dao) > (Dao2_PV))
                        dao2_sv = dao1_sv + Dao_Dao;

                    if ((dao1_sv + RongA) < (Dao3_Min))
                    {
                        dao2_sv = dao1_sv + RongA;
                        dao2_U = 100;

                        // Xả 2 - Dao 4,5
                        if (Dao4_Max < RongTotal / 2)                   //Dao4_Max > 0
                        {
                            dao5_sv = RongTotal / 2;
                            dao5_U = 100;

                            if ((dao5_sv - Dao_Dao) < (Dao4_PV))
                                dao4_sv = dao5_sv - Dao_Dao;
                        }
                        if (Dao4_Max > RongTotal / 2)                   //Dao4_Max > 0
                        {
                            dao4_sv = RongTotal / 2;
                            dao4_U = 100;
                            if ((dao4_sv + Dao_Dao) > (Dao5_PV))
                                dao5_sv = dao4_sv + Dao_Dao;
                        }
                    }


                }

                    
                if (Dao2_Max > RongTotal / 2)                   //Dao2_Max > 0
                {
                    dao2_sv = -1 * RongTotal / 2;
                    dao2_U = 100;
                    if ((dao2_sv - Dao_Dao) < (Dao1_PV))
                        dao1_sv = dao2_sv - Dao_Dao;
                }

                if (Math.Abs(-1 * RongTotal / 2 + RongA) < Math.Abs(Dao3_Min))
                {
                    dao3_sv = -1 * RongTotal / 2 + RongA;
                    dao3_U = 100;

                    // Xả 2 - Dao 4,5
                    if (Dao4_Max < RongTotal / 2)                   //Dao4_Max > 0
                    {
                        dao5_sv = RongTotal / 2;
                        dao5_U = 100;

                        if ((dao5_sv - Dao_Dao) < (Dao4_PV))
                            dao4_sv = dao5_sv - Dao_Dao;
                    }
                    if (Dao4_Max > RongTotal / 2)                   //Dao4_Max > 0
                    {
                        dao4_sv = RongTotal / 2;
                        dao4_U = 100;
                        if ((dao4_sv + Dao_Dao) > (Dao5_PV))
                            dao5_sv = dao4_sv + Dao_Dao;
                    }

                }

                if ((-1 * RongTotal / 2 + RongA) > (Dao3_Max))
                {
                    dao3_sv = 0;
                    dao3_U = 0;

                    // Xả 2 - Dao 4,5
                        dao5_sv = RongTotal / 2;
                        dao5_U = 100;
                        dao4_sv = RongTotal / 2 - RongB;
                        dao4_U = 100;
                }
            }


            // Xả 333
            if (Xa == 3)
            {
                dao3_sv = 0;
                dao1_sv = -1 * (RongTotal / 2);
                dao5_sv = RongTotal / 2;

                dao2_sv = -1 * (RongTotal / 2 - RongA);
                dao4_sv = RongTotal / 2 - RongA;
                dao3_sv = 0;
                dao1_U = 100;
                dao2_U = 100;
                dao3_U = 0;
                dao4_U = 100;
                dao5_U = 100;
            }


            if (Xa == 4)
            {
                dao1_sv = -1 * (RongTotal / 2);
                dao5_sv = RongTotal / 2;

                dao2_sv = -1 * (RongTotal / 2 - RongA);
                dao4_sv = RongTotal / 2 - RongA;
                dao3_sv = 0;
                dao1_U = 100;
                dao2_U = 100;
                dao3_U = 100;
                dao4_U = 100;
                dao5_U = 100;

            }



            if (Lang != 0)                  // Lằng có sử dụng
            {
                if (CaoA != 0)
                {
                    SoLangA++;
                    LA1 = -1 * (RongTotal / 2) + Nap1A;
                }

                if (Nap2A != 0)
                {
                    SoLangA++;
                    LA2 = LA1 + CaoA;
                }

                if (Nap3A != 0)
                {
                    SoLangA++;
                    LA3 = LA2 + Nap2A;
                }
                if (Nap4A != 0)
                {
                    SoLangA++;
                    LA4 = LA3 + Nap3A;
                }


                if (CaoB != 0)
                {
                    SoLangB++;
                    
                    LB1 = -1 * (RongTotal / 2) + RongA + Nap1B;
                }

                if (Nap2B != 0)
                {
                    SoLangB++;
                    LB2 = LB1 + CaoB;
                }

                if (Nap3B != 0)
                {
                    SoLangB++;
                    LB3 = LB2 + Nap2B;
                }
                if (Nap4B != 0)
                {
                    SoLangB++;
                    LB4 = LB3 + Nap3B;
                }

                if (CaoC != 0)
                {
                    SoLangC++;
                    LC1 = -1 * (RongTotal / 2) + RongA + RongB + Nap1C;
                }

                if (Nap2C != 0)
                {
                    SoLangC++;
                    LC2 = LC1 + CaoC;
                }

                if (Nap3C != 0)
                {
                    SoLangC++;
                    LC3 = LC2 + Nap2C;
                }
                if (CaoD != 0)
                {
                    SoLangD++;
                    LD1 = -1 * (RongTotal / 2) + RongA + RongB + RongC + Nap1D;
                }

                if (Nap2D != 0)
                {
                    SoLangD++;
                    LD2 = LD1 + CaoD;
                }

                if (Nap3D != 0)
                {
                    SoLangD++;
                    LD3 = LD2 + Nap2D;
                }
                TongSoLang = SoLangA + SoLangB + SoLangC + SoLangD;
            }

            int[] Array = { LA1, LA2, LA3, LA4, LB1, LB2, LB3, LB4, LC1, LC2, LC3, LD1, LD2, LD3 };
            int[] LangArray = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            // Sắp xếp Lằng
            for (i = 0; i < 14; i++)            
            {
                if (Array[i] != 0)
                    LangArray[i] = Array[i];

                if (Array[i] == 0)
                {
                    for (j = i + 1; j < 14; j++)
                    {
                        if (Array[j] != 0)
                        {
                            n = Array[j];       
                            LangArray[i] = Array[j];
                            m = LangArray[i];
                            Array[j] = 0;
                            n = Array[j];
                        }
                        if (LangArray[i] != 0)
                            break;
                    }
                }
            }
            int[] LangAm = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] LangDuong = { 0, 0, 0, 0, 0, 0, 0, 0 };

            // Tách Lằng Âm / Dương
            j = 0;
            for (i = 0; i < 8; i++)
            {
                if (LangArray[i] < 0)
                {
                    LangAm[i] = LangArray[i];
                    SL_Am++;
                }

                if (LangArray[i] > 0)
                {
                    LangDuong[j] = LangArray[i];
                    SL_Duong++;
                    j++;
                }
            }

            int[] LangPV_Am = { Lang1_PV, Lang2_PV, Lang3_PV, Lang4_PV, 0 };           
            int[] LangPV_Duong = { Lang5_PV, Lang6_PV, Lang7_PV, Lang8_PV, 0 };

            int[] LangSV_Am = new int[20];
            int[] LangSV_Duong = new int[20];
            int[] LangU_Am = new int[20];
            int[] LangU_Duong = new int[20];

            if (SL_Am == 4) 
            {
                LangAm.CopyTo(LangSV_Am, 0);
                LangU_Am[0] = 100;
                LangU_Am[1] = 100;
                LangU_Am[2] = 100;
                LangU_Am[3] = 100;
            } 
                


            if (SL_Duong == 4)
            {
                LangDuong.CopyTo(LangSV_Duong, 0);
                LangU_Duong[0] = 100;
                LangU_Duong[1] = 100;
                LangU_Duong[2] = 100;
                LangU_Duong[3] = 100;
            }



            // Lằng Âm
            if (SL_Am < 4)
            {
                k = SL_Am;
                m = 0;
                for (i=0; i < SL_Am; i++) 
                {
                    n = 1000;
                    for (j = m; j <= (4-k); j++)
                    {
                        if (n > Math.Abs(LangAm[i] - LangPV_Am[j]))
                        {
                            n = Math.Abs(LangAm[i] - LangPV_Am[j]);
                            m = j;
                        }
                    }
                    LangSV_Am[m] = LangAm[i];
                    l = LangSV_Am[m];
                    LangU_Am[m] = 100;

                    if ((l + Lang_Lang) > LangPV_Am[m + 1])
                    {
                        LangPV_Am[m + 1] = l + Lang_Lang;
                        LangSV_Am[m + 1] = l + Lang_Lang;
                    }            
                                                                    
                    l = LangPV_Am[m + 1];
  
                    if (m<3)
                        m++;                   
                    if(k > 0)
                        k--;

                    if ((LangAm[i] == 0)||(k==0))
                        break;
                }  
             }

// Lằng Dương
            if (SL_Duong < 4)
            {
                k = SL_Duong;
                m = 0;
                for (i = 0; i < SL_Duong; i++)
                {
                    n = 1000;
                    for (j = m; j <= (4 - k); j++)
                    {
                        if (n > Math.Abs(LangDuong[i] - LangPV_Duong[j]))
                        {
                            n = Math.Abs(LangDuong[i] - LangPV_Duong[j]);
                            m = j;
                        }
                    }
                    LangSV_Duong[m] = LangDuong[i];
                    l = LangSV_Duong[m];
                    LangU_Duong[m] = 100;

                    if ((l + Lang_Lang) > LangPV_Duong[m + 1])
                    {
                        LangPV_Duong[m + 1] = l + Lang_Lang;
                        LangSV_Duong[m + 1] = l + Lang_Lang;
                    }

                    l = LangPV_Duong[m + 1];

                    if (m < 3)
                        m++;
                    if (k > 0)
                        k--;

                    if ((LangDuong[i] == 0) || (k == 0))
                        break;
                }
            }




            if (may==1)
            {
                lang1_sv = LangSV_Am[0];
                lang5_sv = LangSV_Am[1];
                lang2_sv = LangSV_Am[2];
                lang6_sv = LangSV_Am[3];

                lang3_sv = LangSV_Duong[0];
                lang7_sv = LangSV_Duong[1];
                lang4_sv = LangSV_Duong[2];
                lang8_sv = LangSV_Duong[3];

                lang1_U = LangU_Am[0];
                lang5_U = LangU_Am[1];
                lang2_U = LangU_Am[2];
                lang6_U = LangU_Am[3];

                lang3_U = LangU_Duong[0];
                lang7_U = LangU_Duong[1];
                lang4_U = LangU_Duong[2];
                lang8_U = LangU_Duong[3];
            }

            if (may == 2)
            {
                lang1_sv = LangSV_Am[0];
                lang2_sv = LangSV_Am[1];
                lang3_sv = LangSV_Am[2];
                lang4_sv = LangSV_Am[3];

                lang5_sv = LangSV_Duong[0];
                lang6_sv = LangSV_Duong[1];
                lang7_sv = LangSV_Duong[2];
                lang8_sv = LangSV_Duong[3];

                lang1_U = LangU_Am[0];
                lang2_U = LangU_Am[1];
                lang3_U = LangU_Am[2];
                lang4_U = LangU_Am[3];
                             
                lang5_U = LangU_Duong[0];
                lang6_U = LangU_Duong[1];
                lang7_U = LangU_Duong[2];
                lang8_U = LangU_Duong[3];
            }



            Setting1 = SoLangA;
            Setting2 = SoLangB;
            Setting3 = SoLangC;
            Setting5 = Xa;

 //           System.Windows.MessageBox.Show("Lỗi.! Đơn Hàng Sai", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);


            //May2Tags.Instance.STT2
            //double ketQua = May1Tags.Instance.Lang6_U.Value + 200;
            //dao2_sv = (int)ketQua;

            //if (string.IsNullOrEmpty(error1))
            //    MessageBox.Show("Nạp thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            //else
            //    MessageBox.Show($"{error1}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
