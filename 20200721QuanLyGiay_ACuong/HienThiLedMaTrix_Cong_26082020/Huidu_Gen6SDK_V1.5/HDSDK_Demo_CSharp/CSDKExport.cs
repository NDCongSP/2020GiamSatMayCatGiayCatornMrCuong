using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HDSDK_Demo_CSharp
{
    class CSDKExport
    {
        // Get last error
        [DllImport("HDSDK.dll", EntryPoint = "Hd_GetSDKLastError", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_GetSDKLastError();

        // Create screen
        [DllImport("HDSDK.dll", EntryPoint = "Hd_CreateScreen", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_CreateScreen(int nWidth, int nHeight, int nColor, int nGray, int nCardType, IntPtr pExParamsBuf, int nBufSize);

        // Add program 
        [DllImport("HDSDK.dll", EntryPoint = "Hd_AddProgram", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_AddProgram(IntPtr pBoderImgPath, int nBorderEffect, int nBorderSpeed, IntPtr pExParamsBuf, int nBufSize);

        // Add area
        [DllImport("HDSDK.dll", EntryPoint = "Hd_AddArea", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_AddArea(int nProgramID, int nX, int nY, int nWidth, int nHeight, IntPtr pBoderImgPath, int nBorderEffect, int nBorderSpeed, IntPtr pExParamsBuf, int nBufSize);

        // Add image area item
        [DllImport("HDSDK.dll", EntryPoint = "Hd_AddImageAreaItem", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_AddImageAreaItem(int nAreaID, IntPtr pPaths, int nShowEffect, int nShowSpeed, int nClearType, int nStayTime, IntPtr pExParamsBuf, int nBufSize);

        // Add text area item
        [DllImport("HDSDK.dll", EntryPoint = "Hd_AddSimpleTextAreaItem", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_AddSimpleTextAreaItem(int nAreaID, IntPtr pText, int nTextColor, int nBackGroupColor, int nStyle, IntPtr pFontName, int nFontHeight, int nShowEffect, int nShowSpeed, int nClearType, int nStayTime, IntPtr pExParamsBuf, int nBufSize);

        // Add time area item
        [DllImport("HDSDK.dll", EntryPoint = "Hd_AddTimeAreaItem", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_AddTimeAreaItem(int nAreaID, int nShowMode, int bShowDate, int nDateStyle, int bShowWeek, int nWeekStyle, int bShowTime, int nTimeStyle, int nTextColor, IntPtr pFontName, int nFontHeight, int nDiffHour, int nDiffMin, IntPtr pExParamsBuf, int nBufSize);

        // Create realtime area item
        [DllImport("HDSDK.dll", EntryPoint = "Hd_CreateRealtimeArea", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_CreateRealtimeArea(int nX, int nY, int nWidth, int nHeight, IntPtr pImgPath, int nUseRealTime, int bOnlyShowRealtimeArea, int bSave, int nLivetime, IntPtr pExParamsBuf, int nBufSize);

        // Send command
        [DllImport("HDSDK.dll", EntryPoint = "Hd_SendCommand", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_SendCommand(int nSendType, IntPtr pStrParams, int nCommandType, IntPtr pConText, int nTextLen, IntPtr pDeviceGUID, IntPtr pOutConText, ref int pOutConTextLen, IntPtr pExParamsBuf, int nBufSize);

        // Send screen
        [DllImport("HDSDK.dll", EntryPoint = "Hd_SendScreen", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_SendScreen(int nSendType, IntPtr pStrParams, IntPtr pDeviceGUID, IntPtr pExParamsBuf, int nBufSize);

        // Send realtime area
        [DllImport("HDSDK.dll", EntryPoint = "Hd_SendRealTimeArea", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_SendRealTimeArea(int nSendType, IntPtr pStrParams, IntPtr pDeviceGUID, IntPtr pExParamsBuf, int nBufSize);

        // Get Color value
        [DllImport("HDSDK.dll", EntryPoint = "Hd_GetColor", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_GetColor(int r, int g, int b);

        // Save screen to disk
        [DllImport("HDSDK.dll", EntryPoint = "Hd_SaveScreen", CallingConvention = CallingConvention.StdCall)]
        public static extern int Hd_SaveScreen(IntPtr pDirPath);

        // command 
        // Get baudrate
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_GetBaudRate", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_GetBaudRate(int nPort, ref int pBaudRate, IntPtr pDeviceGUID);

        // Set baudrate
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_SetBaudRate", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_SetBaudRate(int nPort, int nSrcBaudRate, int nDestBaudRate, IntPtr pDeviceGUID);

        // Is card online
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_IsCardOnline", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_IsCardOnline(int nSendType, IntPtr pStrParams, IntPtr pDeviceGUID);

        // Clear screen
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_ClearScreen", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_ClearScreen(int nSendType, IntPtr pStrParams, IntPtr pDeviceGUID);

        // Restart card
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_RestartCard", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_RestartCard(int nSendType, IntPtr pStrParams, IntPtr pDeviceGUID);

        // Test screen
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_TestScreen", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_TestScreen(int nSendType, IntPtr pStrParams, int nStyle, IntPtr pDeviceGUID);

        // adjust time
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_AdjustTime", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_AdjustTime(int nSendType, IntPtr pStrParams, IntPtr pDeviceGUID);

        // set luminance
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_SetLuminance", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_SetLuminance(int nSendType, IntPtr pStrParams, int nLuminance, IntPtr pDeviceGUID);

        // screen ctrl
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_ScreenCtrl", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_ScreenCtrl(int nSendType, IntPtr pStrParams, int nCtrlCode, IntPtr pDeviceGUID);

        // timeswitch
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_TimeSwitch", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_TimeSwitch(int nSendType, IntPtr pStrParams, int nUse, int nBootuUpTime, int nShutDownTime, IntPtr pDeviceGUID);

        // switch program
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_SwitchProgram", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_SwitchProgram(int nSendType, IntPtr pStrParams, int nProgramIndex, IntPtr pDeviceGUID);

        // Set ip
        [DllImport("HDSDK.dll", EntryPoint = "Cmd_SetIP", CallingConvention = CallingConvention.StdCall)]
        public static extern int Cmd_SetIP(IntPtr pSrcIP, IntPtr pDestIP, IntPtr pDestMask, IntPtr pDestGateway, IntPtr pDeviceGUID);
    }
}
