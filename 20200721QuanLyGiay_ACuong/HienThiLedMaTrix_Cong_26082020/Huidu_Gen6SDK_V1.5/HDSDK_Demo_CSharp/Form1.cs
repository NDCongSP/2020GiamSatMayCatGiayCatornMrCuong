using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HDSDK_Demo_CSharp
{
    public partial class FormSDK : Form
    {

        int m_nSendType;                // Send type
        IntPtr m_pSendParams;           // Params

        public FormSDK()
        {
            InitializeComponent();

            // init send type and send params

            bool bSendUseIp = true;  

            // IP
            if (bSendUseIp)
            {
                m_nSendType = 0;
                string strParams = "192.168.2.201";
                m_pSendParams = Marshal.StringToHGlobalUni(strParams);
            }
            else
            {
                // Serial port see Cmd_GetBaudRate Cmd_SetBaudRate
                int nSerialPort = 4;
                int nBaudRate = 57600;
                string strParams = nSerialPort.ToString() + ":" + nBaudRate.ToString();

                m_nSendType = 1;
                m_pSendParams = Marshal.StringToHGlobalUni(strParams);
            }
        }

        ~FormSDK()
        {
            Marshal.FreeHGlobal(m_pSendParams);
        }

        // Text
        private void buttonText_Click(object sender, EventArgs e)
        {
            IntPtr pNULL = new IntPtr(0);

            int nErrorCode = -1;
            // 1. Create a screen
	        int nWidth = 64;
	        int nHeight = 32;
	        int nColor = 1;
	        int nGray = 1;
	        int nCardType = 0;

            int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
	        if (nRe != 0)
	        {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
	        }

	        // 2. Add program to screen
            int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);
	        if (nProgramID == -1)
	        {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
	        }

	        int nX = 0;
	        int nY = 0;
	        int nAreaWidth = 64;
	        int nAreaHeight = 32;

	        // 3. Add Area to program
            int nAreaID = CSDKExport.Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
	        if (nAreaID == -1)
	        {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
		       return;
	        }

	        // 4.Add text AreaItem to Area
            IntPtr pText = Marshal.StringToHGlobalUni("12345");
	        IntPtr pFontName = Marshal.StringToHGlobalUni("Arial");
            int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);
            int nAreaItemID = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID, pText, nTextColor, 0, 4, pFontName, 16, 0, 30, 201, 3, pNULL, 0);
	        if (nAreaItemID == -1)
	        {
                Marshal.FreeHGlobal(pText);
                Marshal.FreeHGlobal(pFontName);
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
		        return;
	        }
            Marshal.FreeHGlobal(pText);
            Marshal.FreeHGlobal(pFontName);

	        // 5. Send to device 
            nRe = CSDKExport.Hd_SendScreen(m_nSendType, m_pSendParams, pNULL, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
            }
        }

        // Image
        private void buttonImage_Click(object sender, EventArgs e)
        {

            IntPtr pNULL = new IntPtr(0);

            int nErrorCode = -1;
            // 1. Create a screen
            int nWidth = 64;
            int nHeight = 32;
            int nColor = 1;
            int nGray = 1;
            int nCardType = 0;

            int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }

            // 2. Add program to screen
            int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);
            if (nProgramID == -1)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }

            int nX = 0;
            int nY = 0;
            int nAreaWidth = 64;
            int nAreaHeight = 32;

            // 3. Add Area to program
            int nAreaID = CSDKExport.Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
            if (nAreaID == -1)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }

            // 4.Add image AreaItem to Area
            IntPtr pPath = Marshal.StringToHGlobalUni("d:\\test.bmp");
            int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);
            int nAreaItemID = CSDKExport.Hd_AddImageAreaItem(nAreaID, pPath, 0, 30, 201, 3, pNULL, 0);
            if (nAreaItemID == -1)
            {
                Marshal.FreeHGlobal(pPath);
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }
            Marshal.FreeHGlobal(pPath);
  

            // 5. Send to device 
            nRe = CSDKExport.Hd_SendScreen(m_nSendType, m_pSendParams, pNULL, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
            }
        }

        // Time
        private void buttonTime_Click(object sender, EventArgs e)
        {
            IntPtr pNULL = new IntPtr(0);

            int nErrorCode = -1;
            // 1. Create a screen
            int nWidth = 64;
            int nHeight = 32;
            int nColor = 1;
            int nGray = 1;
            int nCardType = 0;

            int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }

            // 2. Add program to screen
            int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);
            if (nProgramID == -1)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }

            int nX = 0;
            int nY = 0;
            int nAreaWidth = 64;
            int nAreaHeight = 32;

            // 3. Add Area to program
            int nAreaID = CSDKExport.Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
            if (nAreaID == -1)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }


            // 4.Add time AreaItem to Area
            IntPtr pFontName = Marshal.StringToHGlobalUni("Arial");
            int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);
            int nAreaItemID = CSDKExport.Hd_AddTimeAreaItem(nAreaID, 0, 1, 0, 0, 0, 0, 0, nTextColor, pFontName, 16, 0, 0, pNULL, 0);
            if (nAreaItemID == -1)
            {
                Marshal.FreeHGlobal(pFontName);
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }
            Marshal.FreeHGlobal(pFontName);

            // 5. Send to device 
            nRe = CSDKExport.Hd_SendScreen(m_nSendType, m_pSendParams, pNULL, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
            }
        }

        // RealTimeArea
        private void buttonRealTimeArea_Click(object sender, EventArgs e)
        {

            IntPtr pNULL = new IntPtr(0);

            int nErrorCode = -1;
            // 1. Create a screen
            int nWidth = 64;
            int nHeight = 32;
            int nColor = 1;
            int nGray = 1;
            int nCardType = 0;

            int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
                return;
            }

            // 2. CreateRealtimeArea
	        int nX = 0;
	        int nY = 16;
	        int nAreaWidth = 64;
	        int nAreaHeight = 16;
            IntPtr pPath = Marshal.StringToHGlobalUni("d:\\test.bmp");
            nRe = CSDKExport.Hd_CreateRealtimeArea(nX, nY, nAreaWidth, nAreaHeight, pPath, 1, 0, 0, 0, pNULL, 0);
	        if (nRe != 0)
	        {
                Marshal.FreeHGlobal(pPath);
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
	        }

            // 3. Send to device 
            nRe = CSDKExport.Hd_SendRealTimeArea(m_nSendType, m_pSendParams, pNULL, pNULL, 0);
            if (nRe != 0)
            {
                nErrorCode = CSDKExport.Hd_GetSDKLastError();
            }
            Marshal.FreeHGlobal(pPath);
        }

        // Command
        private void buttonCmd_Click(object sender, EventArgs e)
        {

            IntPtr pNULL = new IntPtr(0);

            // 1 check is card online.
            int nRe = CSDKExport.Cmd_IsCardOnline(m_nSendType, m_pSendParams, pNULL);

            // 2 clear screen
            nRe = CSDKExport.Cmd_ClearScreen(m_nSendType, m_pSendParams, pNULL);


            //// Get and Set baudrate
            //int nRate = 0;
            //nRe = CSDKExport.Cmd_GetBaudRate(4, ref nRate, pNULL);
            //if (nRe == 0)
            //{
            //    nRe = CSDKExport.Cmd_SetBaudRate(4, nRate, 57600, pNULL);
            //}
        }
    }
}
