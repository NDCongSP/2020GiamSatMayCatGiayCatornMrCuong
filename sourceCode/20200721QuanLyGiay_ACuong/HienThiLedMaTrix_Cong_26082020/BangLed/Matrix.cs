using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace BangLed
{
    public class Matrix
    {
        #region bien class
        int m_nSendType = 0;// cad dung cong lan
        IntPtr m_pSendParams;// string ip lan
        // đường dẫn hình viền vùng
        public IntPtr pPath= Marshal.StringToHGlobalUni(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "1.png"));// vien cho phan vung
        public string ip = "192.168.1.203";
        //-----------------------------------//
        //---------------man hình chính--------------------//
        public int ScreenWidth = 320;// chiều ngang
        public int ScreenHeight = 112;// chiều cao
        public int ScreenColor = 0; // 0 đơn màu, 1 ba màu, 2 full màu
        public int ScreenGray = 1;
        public int ScreenCardType = 0;
        //---------------vung thứ 1- Title--------------------//
        public int Area1X = 0;//tọa độ x
        public int Area1Y = 0;//tọa độ y
        public int Area1Width = 160; // chiều ngang
        public int Area1Height = 23;// chiều cao
        public string Area1Text = "456";// nội dung
        public string Area1FontName = "Cambria";
        public int Area1FontSize = 15; // cỡ chữ
        public int Area1Stype = 260; // in đậm và canh vị trí chữ
        public int Area1ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area1ShowSpeed = 30;
        //---------------vung thứ 2--------------------//
        public int Area2X = 0;//tọa độ x
        public int Area2Y = 22;//tọa độ y
        public int Area2Width = 160; // chiều ngang
        public int Area2Height = 71;// chiều cao
        public string Area2Text = " ";// nội dung
        public string Area2FontName = "Cambria";
        public int Area2FontSize = 14; // cỡ chữ
        public int Area2Stype = 260; // in đậm và canh vị trí chữ
        public int Area2ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area2ShowSpeed = 120;
        //---------------vung thứ 3--------------------//
        public int Area3X = 0;//tọa độ x
        public int Area3Y = 92;//tọa độ y
        public int Area3Width = 160; // chiều ngang
        public int Area3Height = 20;// chiều cao
        public string Area3Text = " ";// nội dung
        public string Area3FontName = "Cambria";
        public int Area3FontSize = 14; // cỡ chữ
        public int Area3Stype = 260; // in đậm và canh vị trí chữ
        public int Area3ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area3ShowSpeed = 120;
        //---------------vung thứ 4 - Tốc độ lớn--------------------//
        public int Area4X = 1;//tọa độ x
        public int Area4Y = 23;//tọa độ y
        public int Area4Width = 73; // chiều ngang
        public int Area4Height = 43;// chiều cao
        public string Area4Text = "";// nội dung
        public string Area4FontName = "Cambria";
        public int Area4FontSize = 36; // cỡ chữ
        public int Area4Stype = 260; // in đậm và canh vị trí chữ
        public int Area4ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area4ShowSpeed = 120;
        //---------------vung thứ 5 - Còn lại--------------------//
        public int Area5X = 74;//tọa độ x
        public int Area5Y = 23;//tọa độ y
        public int Area5Width = 83; // chiều ngang
        public int Area5Height = 21;// chiều cao
        public string Area5Text = "";// nội dung
        public string Area5FontName = "Cambria";
        public int Area5FontSize = 16; // cỡ chữ
        public int Area5Stype = 261; // in đậm và canh vị trí chữ
        public int Area5ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area5ShowSpeed = 120;
        //---------------vung thứ 6- Số mét tổng--------------------//
        public int Area6X = 74;//tọa độ x
        public int Area6Y = 44;//tọa độ y
        public int Area6Width = 83; // chiều ngang
        public int Area6Height = 22;// chiều cao
        public string Area6Text = "";// nội dung
        public string Area6FontName = "Cambria";
        public int Area6FontSize = 16; // cỡ chữ
        public int Area6Stype = 261; // in đậm và canh vị trí chữ
        public int Area6ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area6ShowSpeed = 120;
        //---------------vung thứ 7--------------------//
        public int Area7X = 6;//tọa độ x
        public int Area7Y = 66;//tọa độ y
        public int Area7Width = 33; // chiều ngang
        public int Area7Height = 26;// chiều cao
        public string Area7Text = "T.Độ";// nội dung
        public string Area7FontName = "Cambria";
        public int Area7FontSize = 12; // cỡ chữ
        public int Area7Stype = 259; // in đậm và canh vị trí chữ
        public int Area7ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area7ShowSpeed = 120;
        //---------------vung thứ 8- Tốc độ nhỏ--------------------//
        public int Area8X = 39;//tọa độ x
        public int Area8Y = 66;//tọa độ y
        public int Area8Width = 46; // chiều ngang
        public int Area8Height = 26;// chiều cao
        public string Area8Text = "";// nội dung
        public string Area8FontName = "Cambria";
        public int Area8FontSize = 16; // cỡ chữ
        public int Area8Stype = 259; // in đậm và canh vị trí chữ
        public int Area8ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area8ShowSpeed = 120;
        //---------------vung thứ 9 Phần trăm--------------------//
        public int Area9X = 85;//tọa độ x
        public int Area9Y = 66;//tọa độ y
        public int Area9Width = 72; // chiều ngang
        public int Area9Height = 26;// chiều cao
        public string Area9Text = "";// nội dung
        public string Area9FontName = "Cambria";
        public int Area9FontSize = 16; // cỡ chữ
        public int Area9Stype = 261; // in đậm và canh vị trí chữ
        public int Area9ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area9ShowSpeed = 120;
        //---------------vung thứ 10 Thời gian bên trái--------------------//
        public int Area10X = 6;//tọa độ x
        public int Area10Y = 93;//tọa độ y
        public int Area10Width = 147; // chiều ngang
        public int Area10Height = 18;// chiều cao
        public string Area10Text = "";// nội dung
        public string Area10FontName = "Cambria";
        public int Area10FontSize = 12; // cỡ chữ
        public int Area10Stype = 260; // in đậm và canh vị trí chữ
        public int Area10ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        public int Area10ShowSpeed = 120;
        //---------------vung thứ 11 so ở giữa--------------------//
        //public int Area11X = 64;//tọa độ x
        //public int Area11Y = 93;//tọa độ y
        //public int Area11Width = 32; // chiều ngang
        //public int Area11Height = 18;// chiều cao
        public string Area11Text = "";// nội dung
        //public string Area11FontName = "Cambria";
        //public int Area11FontSize = 12; // cỡ chữ
        //public int Area11Stype = 260; // in đậm và canh vị trí chữ
        //public int Area11ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        //public int Area11ShowSpeed = 120;
        ////---------------vung thứ 12 thoi gian ben phải--------------------//
        //public int Area12X = 96;//tọa độ x
        //public int Area12Y = 93;//tọa độ y
        //public int Area12Width = 57; // chiều ngang
        //public int Area12Height = 18;// chiều cao
        public string  Area12Text = "";// nội dung
        //public string  Area12FontName = "Cambria";
        //public int Area12FontSize = 12; // cỡ chữ
        //public int Area12Stype = 261; // in đậm và canh vị trí chữ
        //public int Area12ShowEffect = 0;// 0 đứng yen, 202 di chuyển
        //public int Area12ShowSpeed = 120;
        #endregion
        private void addArea(int _nProgramID,int _AreaX, int _AreaY, int _AreaWidth, int _AreaHeight, IntPtr _pPath, IntPtr _pNULL,int _nErrorCode, string _AreaText, string _AreaFontName, int _AreaFontSize,int _nTextColor,int _AreaStype,int _AreaShowEffect,int _AreaShowSpeed,string _donvi, int borderEffect = 3)
        {
            #region nap phan vung
            // thêm phân vùng 1
            int nAreaID = CSDKExport.Hd_AddArea(_nProgramID, _AreaX, _AreaY, _AreaWidth, _AreaHeight, _pPath, borderEffect, 5, _pNULL, 0);
            if (nAreaID == -1)
            {
                _nErrorCode = CSDKExport.Hd_GetSDKLastError();
                //MessageBox.Show("Bug3 :" + _nErrorCode.ToString());
                return;
            }
            // thêm nội dung hiển thị vào phân vùng
            IntPtr pText = Marshal.StringToHGlobalUni(_AreaText+_donvi);
            IntPtr pFontName = Marshal.StringToHGlobalUni(_AreaFontName);
            //----------------------------------------------------Phan vùng|nội dung|màu|nền|kiểu text|   fontname|font size  |kiểu dịch text|tốc độ dịch|201| 0| pNULL| 0                                                  
            int nAreaItemID1 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID, pText, _nTextColor, 0, _AreaStype, pFontName, Convert.ToInt32(_AreaFontSize), _AreaShowEffect, _AreaShowSpeed, 201, 0, _pNULL, 0);
            if (nAreaItemID1 == -1)
            {
                Marshal.FreeHGlobal(pText);
                Marshal.FreeHGlobal(pFontName);
                _nErrorCode = CSDKExport.Hd_GetSDKLastError();
                //MessageBox.Show("Bug4 :" + _nErrorCode.ToString());
                return;
            }
            Marshal.FreeHGlobal(pText);
            Marshal.FreeHGlobal(pFontName);
            //MessageBox.Show("text:" + _AreaText + _donvi + "|X:" + _AreaX.ToString() + "|Y:" + _AreaY.ToString() + "|W:" + _AreaWidth.ToString()
            //    + "|H:" + _AreaHeight.ToString() + "|fSIZE:" + _AreaFontSize.ToString() + "|CHAY:" + _AreaShowEffect.ToString());
            #endregion
        }
        public  void Sendata()
        {
            try
            {
                int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);
                m_pSendParams = Marshal.StringToHGlobalUni(ip);
                IntPtr pNULL = new IntPtr(0);
                int nErrorCode = -1;
                // tạo màn hình chính
                int nRe = CSDKExport.Hd_CreateScreen(ScreenWidth, ScreenHeight, ScreenColor, ScreenGray, ScreenCardType, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    return;
                }
                // thêm chương trình  1 vào màn hình
                int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);//viền,kiểu chạy viền,tốc độ viền,0,0
                if (nProgramID == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    return;
                }
                // 3. thêm phân vùng vào chương trình
                #region nap vung 1
                //// thêm phân vùng 1
                //int nAreaID1 = CSDKExport.Hd_AddArea(nProgramID, Area1X, Area1Y, Area1Width, Area1Height, pPath, 3, 5, pNULL, 0);
                //if (nAreaID1 == -1)
                //{
                //    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                //    return;
                //}
                //// thêm nội dung hiển thị vào phân vùng
                //IntPtr pText1 = Marshal.StringToHGlobalUni(Area1Text);
                //IntPtr pFontName1 = Marshal.StringToHGlobalUni(Area1FontName);
                ////----------------------------------------------------Phan vùng|nội dung|màu|nền|kiểu text|   fontname|font size  |kiểu dịch text|tốc độ dịch|201| 0| pNULL| 0                                                  
                //int nAreaItemID1 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID1, pText1, nTextColor, 0, Area1Stype, pFontName1, Convert.ToInt32(Area1FontSize), Area1ShowEffect, Area1ShowSpeed, 201, 0, pNULL, 0);
                //if (nAreaItemID1 == -1)
                //{
                //    Marshal.FreeHGlobal(pText1);
                //    Marshal.FreeHGlobal(pFontName1);
                //    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                //    //MessageBox.Show("Bug4 :" + nErrorCode.ToString());
                //    return;
                //}
                //Marshal.FreeHGlobal(pText1);
                //Marshal.FreeHGlobal(pFontName1);
                #endregion
                // MẶT 1
                addArea(nProgramID, Area1X, Area1Y, Area1Width, Area1Height, pPath, pNULL, nErrorCode, Area1Text, Area1FontName, Area1FontSize, nTextColor, Area1Stype, Area1ShowEffect, Area1ShowSpeed, "");
                addArea(nProgramID, Area2X, Area2Y, Area2Width, Area2Height, pPath, pNULL, nErrorCode, Area2Text, Area2FontName, Area2FontSize, nTextColor, Area2Stype, Area2ShowEffect, Area2ShowSpeed, "");
                addArea(nProgramID, Area3X, Area3Y, Area3Width, Area3Height, pPath, pNULL, nErrorCode, Area3Text, Area3FontName, Area3FontSize, nTextColor, Area3Stype, Area3ShowEffect, Area3ShowSpeed, "");
                Thread.Sleep(1000);
                addArea(nProgramID, Area4X, Area4Y, Area4Width, Area4Height, pNULL, pNULL, nErrorCode, Area4Text, Area4FontName, Area4FontSize, nTextColor, Area4Stype, Area4ShowEffect, Area4ShowSpeed, "");
                addArea(nProgramID, Area5X, Area5Y, Area5Width, Area5Height, pNULL, pNULL, nErrorCode, Area5Text, Area5FontName, Area5FontSize, nTextColor, Area5Stype, Area5ShowEffect, Area5ShowSpeed, " CL");
                addArea(nProgramID, Area6X, Area6Y, Area6Width, Area6Height, pNULL, pNULL, nErrorCode, Area6Text, Area6FontName, Area6FontSize, nTextColor, Area6Stype, Area6ShowEffect, Area6ShowSpeed, " MT");
                addArea(nProgramID, Area7X, Area7Y, Area7Width, Area7Height, pNULL, pNULL, nErrorCode, Area7Text, Area7FontName, Area7FontSize, nTextColor, Area7Stype, Area7ShowEffect, Area7ShowSpeed, "");
                addArea(nProgramID, Area8X, Area8Y, Area8Width, Area8Height, pNULL, pNULL, nErrorCode, Area8Text, Area8FontName, Area8FontSize, nTextColor, Area8Stype, Area8ShowEffect, Area8ShowSpeed, "");
                addArea(nProgramID, Area9X, Area9Y, Area9Width, Area9Height, pNULL, pNULL, nErrorCode, Area9Text, Area9FontName, Area9FontSize, nTextColor, Area9Stype, Area9ShowEffect, Area9ShowSpeed, " %");
                addArea(nProgramID, Area10X, Area10Y, Area10Width, Area10Height, pNULL, pNULL, nErrorCode, Area10Text + "     " + Area11Text + "     " + Area12Text, Area10FontName, Area10FontSize, nTextColor, Area10Stype, Area10ShowEffect, Area10ShowSpeed, "");


                //addArea(nProgramID, Area11X, Area11Y, Area11Width, Area11Height, pNULL, pNULL, nErrorCode, Area11Text, Area11FontName, Area11FontSize, nTextColor, Area11Stype, Area11ShowEffect, Area11ShowSpeed, "");
                //addArea(nProgramID, Area12X, Area12Y, Area12Width, Area12Height, pNULL, pNULL, nErrorCode, Area12Text, Area12FontName, Area12FontSize, nTextColor, Area12Stype, Area12ShowEffect, Area12ShowSpeed, "");
                // MẶT 2
                addArea(nProgramID, Area1X + 160, Area1Y, Area1Width, Area1Height, pPath, pNULL, nErrorCode, Area1Text, Area1FontName, Area1FontSize, nTextColor, Area1Stype, Area1ShowEffect, Area1ShowSpeed, "");
                addArea(nProgramID, Area2X + 160, Area2Y, Area2Width, Area2Height, pPath, pNULL, nErrorCode, Area2Text, Area2FontName, Area2FontSize, nTextColor, Area2Stype, Area2ShowEffect, Area2ShowSpeed, "");
                addArea(nProgramID, Area3X + 160, Area3Y, Area3Width, Area3Height, pPath, pNULL, nErrorCode, Area3Text, Area3FontName, Area3FontSize, nTextColor, Area3Stype, Area3ShowEffect, Area3ShowSpeed, "");
                addArea(nProgramID, Area4X + 160, Area4Y, Area4Width, Area4Height, pNULL, pNULL, nErrorCode, Area4Text, Area4FontName, Area4FontSize, nTextColor, Area4Stype, Area4ShowEffect, Area4ShowSpeed, "");
                addArea(nProgramID, Area5X + 160, Area5Y, Area5Width, Area5Height, pNULL, pNULL, nErrorCode, Area5Text, Area5FontName, Area5FontSize, nTextColor, Area5Stype, Area5ShowEffect, Area5ShowSpeed, " CL");
                addArea(nProgramID, Area6X + 160, Area6Y, Area6Width, Area6Height, pNULL, pNULL, nErrorCode, Area6Text, Area6FontName, Area6FontSize, nTextColor, Area6Stype, Area6ShowEffect, Area6ShowSpeed, " MT");
                addArea(nProgramID, Area7X + 160, Area7Y, Area7Width, Area7Height, pNULL, pNULL, nErrorCode, Area7Text, Area7FontName, Area7FontSize, nTextColor, Area7Stype, Area7ShowEffect, Area7ShowSpeed, "");
                addArea(nProgramID, Area8X + 160, Area8Y, Area8Width, Area8Height, pNULL, pNULL, nErrorCode, Area8Text, Area8FontName, Area8FontSize, nTextColor, Area8Stype, Area8ShowEffect, Area8ShowSpeed, "");
                addArea(nProgramID, Area9X + 160, Area9Y, Area9Width, Area9Height, pNULL, pNULL, nErrorCode, Area9Text, Area9FontName, Area9FontSize, nTextColor, Area9Stype, Area9ShowEffect, Area9ShowSpeed, " %");
                addArea(nProgramID, Area10X + 160, Area10Y, Area10Width, Area10Height, pNULL, pNULL, nErrorCode, Area10Text + "     " + Area11Text + "     " + Area12Text, Area10FontName, Area10FontSize, nTextColor, Area10Stype, Area10ShowEffect, Area10ShowSpeed, "");
                //addArea(nProgramID, Area11X + 160, Area11Y, Area11Width, Area11Height, pNULL, pNULL, nErrorCode, Area11Text, Area11FontName, Area11FontSize, nTextColor, Area11Stype, Area11ShowEffect, Area11ShowSpeed, "");
                //addArea(nProgramID, Area12X + 160, Area12Y, Area12Width, Area12Height, pNULL, pNULL, nErrorCode, Area12Text, Area12FontName, Area12FontSize, nTextColor, Area12Stype, Area12ShowEffect, Area12ShowSpeed, "");
                // . program to screen
                nRe = CSDKExport.Hd_SendScreen(m_nSendType, m_pSendParams, pNULL, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    //MessageBox.Show("Quá trình cập nhật lỗi. Mã lỗi: " + nErrorCode.ToString());
                }
                //MessageBox.Show("Quá trình cập nhật ok !!!");
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Quá trình cập nhật lỗi. Mã lỗi: " + ex.ToString());
            }
        }
    }
}