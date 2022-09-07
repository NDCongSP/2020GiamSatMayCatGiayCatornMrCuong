using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MayCatServer
{
    public class RealTimeMatrix
    {
        public IntPtr pPath = Marshal.StringToHGlobalUni(
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "1.png"));// vien cho phan vung

        // Ip address
        IntPtr m_pSendParams;
        public string ip = "192.168.1.203";
        int m_nSendType = 0;// card dung cong lan

        //-----------------------------------//
        //---------------man hình chính--------------------//
        public int ScreenWidth = 320;// chiều ngang
        public int ScreenHeight = 112;// chiều cao
        public int ScreenColor = 0; // 0 đơn màu, 1 ba màu, 2 full màu
        public int ScreenGray = 1;
        public int ScreenCardType = 0;

        IntPtr pNULL = new IntPtr(0);

        public void CreateArea()
        {
            // Chữ màu đỏ
            int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);
            m_pSendParams = Marshal.StringToHGlobalUni(ip);
            int nErrorCode = -1;
            // Tạo màn hình chính
            int nRe = CSDKExport.Hd_CreateScreen(ScreenWidth, ScreenHeight, ScreenColor, ScreenGray, ScreenCardType, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }

            // Thêm vùng realtime
            CSDKExport.Hd_CreateRealtimeArea(0, 0, 160, 23, pPath, 1, 0, 0, 0, pNULL, 0);
        }

        public void SendRealTimeData(string text, string _AreaFontName)
        {
            // thêm nội dung hiển thị vào phân vùng
            IntPtr pText = Marshal.StringToHGlobalUni(text);
            IntPtr pFontName = Marshal.StringToHGlobalUni(_AreaFontName);
            // Khởi động vùng realtime
            CSDKExport.Hd_SendRealTimeArea(m_nSendType, pText, pNULL, pNULL, 0);
            Marshal.FreeHGlobal(pText);
            Marshal.FreeHGlobal(pFontName);
        }
    }
}
