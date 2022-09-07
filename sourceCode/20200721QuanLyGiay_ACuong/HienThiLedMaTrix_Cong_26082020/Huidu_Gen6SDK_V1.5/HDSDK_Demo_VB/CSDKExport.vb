Imports System.Runtime.InteropServices
Public Class CSDKExport

    ' Get last error
    <DllImport("HDSDK.dll", EntryPoint:="Hd_GetSDKLastError", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_GetSDKLastError() As Integer
    End Function

    ' Create screen
    <DllImport("HDSDK.dll", EntryPoint:="Hd_CreateScreen", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_CreateScreen(nWidth As Integer, nHeight As Integer, nColor As Integer, nGray As Integer, nCardType As Integer,
                                        pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function

    ' Add program 
    <DllImport("HDSDK.dll", EntryPoint:="Hd_AddProgram", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_AddProgram(pBoderImgPath As IntPtr, nBorderEffect As Integer, nBorderSpeed As Integer, pExParamsBuf As IntPtr,
                                      nBufSize As Integer) As Integer
    End Function

    ' Add area
    <DllImport("HDSDK.dll", EntryPoint:="Hd_AddArea", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_AddArea(nProgramID As Integer, nX As Integer, nY As Integer, nWidth As Integer, nHeight As Integer,
                                   pBoderImgPath As IntPtr, nBorderEffect As Integer, nBorderSpeed As Integer, pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function

    'Add image area item
    <DllImport("HDSDK.dll", EntryPoint:="Hd_AddImageAreaItem", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_AddImageAreaItem(nAreaID As Integer, pPaths As IntPtr, nShowEffect As Integer, nShowSpeed As Integer,
                                            nClearType As Integer, nStayTime As Integer, pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function

    ' Add text area item
    <DllImport("HDSDK.dll", EntryPoint:="Hd_AddSimpleTextAreaItem", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_AddSimpleTextAreaItem(nAreaID As Integer, pText As IntPtr, nTextColor As Integer, nBackGroupColor As Integer,
                                            nStyle As Integer, pFontName As IntPtr, nFontHeight As Integer, nShowEffect As Integer, nShowSpeed As Integer,
                                            nClearType As Integer, nStayTime As Integer, pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function

    ' Add time area item
    <DllImport("HDSDK.dll", EntryPoint:="Hd_AddTimeAreaItem", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_AddTimeAreaItem(nAreaID As Integer, nShowMode As Integer, bShowDate As Integer, nDateStyle As Integer,
                                            bShowWeek As Integer, nWeekStyle As Integer, bShowTime As Integer, nTimeStyle As Integer,
                                             nTextColor As Integer, pFontName As IntPtr, nFontHeight As Integer, nDiffHour As Integer, nDiffMin As Integer,
                                            pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function

    ' Create realtime area item
    <DllImport("HDSDK.dll", EntryPoint:="Hd_CreateRealtimeArea", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_CreateRealtimeArea(nX As Integer, nY As Integer, nWidth As Integer, nHeight As Integer,
                                            pImgPath As IntPtr, nUseRealTime As Integer, bOnlyShowRealtimeArea As Integer, bSave As Integer, nLivetime As Integer,
                                            pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function


    ' Send command
    <DllImport("HDSDK.dll", EntryPoint:="Hd_SendCommand", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_SendCommand(nSendType As Integer, pStrParams As IntPtr, nCommandType As Integer, pConText As IntPtr,
                                            nTextLen As Integer, pDeviceGUID As IntPtr, pOutConText As IntPtr, ByRef pOutConTextLen As Integer, nLivetime As Integer, pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function

    ' Send screen
    <DllImport("HDSDK.dll", EntryPoint:="Hd_SendScreen", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_SendScreen(nSendType As Integer, pStrParams As IntPtr, pDeviceGUID As IntPtr, pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function

    ' Send realtime area
    <DllImport("HDSDK.dll", EntryPoint:="Hd_SendRealTimeArea", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_SendRealTimeArea(nSendType As Integer, pStrParams As IntPtr, pDeviceGUID As IntPtr, pExParamsBuf As IntPtr, nBufSize As Integer) As Integer
    End Function


    ' Get Color value
    <DllImport("HDSDK.dll", EntryPoint:="Hd_GetColor", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_GetColor(r As Integer, g As Integer, b As Integer) As Integer
    End Function

    ' Save screen to disk
    <DllImport("HDSDK.dll", EntryPoint:="Hd_SaveScreen", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Hd_SaveScreen(pDirPath As IntPtr) As Integer
    End Function

    '// command 
    'Get baudrate
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_GetBaudRate", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_GetBaudRate(nPort As Integer, ByRef pBaudRate As Integer, pDeviceGUID As IntPtr) As Integer
    End Function

    '// Set baudrate
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_SetBaudRate", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_SetBaudRate(nPort As Integer, nSrcBaudRate As Integer, nDestBaudRate As Integer, pDeviceGUID As IntPtr) As Integer
    End Function

    ' Is card online
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_IsCardOnline", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_IsCardOnline(nSendType As Integer, pStrParams As IntPtr, pDeviceGUID As IntPtr) As Integer
    End Function

    ' Clear screen
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_ClearScreen", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_ClearScreen(nSendType As Integer, pStrParams As IntPtr, pDeviceGUID As IntPtr) As Integer
    End Function

    ' Restart card
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_RestartCard", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_RestartCard(nSendType As Integer, pStrParams As IntPtr, pDeviceGUID As IntPtr) As Integer
    End Function

    ' Test screen
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_TestScreen", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_TestScreen(nSendType As Integer, pStrParams As IntPtr, nStyle As Integer, pDeviceGUID As IntPtr) As Integer
    End Function

    ' adjust time
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_AdjustTime", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_AdjustTime(nSendType As Integer, pStrParams As IntPtr, pDeviceGUID As IntPtr) As Integer
    End Function

    ' set luminance
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_SetLuminance", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_SetLuminance(nSendType As Integer, pStrParams As IntPtr, nLuminance As Integer, pDeviceGUID As IntPtr) As Integer
    End Function

    ' screen ctrl
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_ScreenCtrl", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_ScreenCtrl(nSendType As Integer, pStrParams As IntPtr, nCtrlCode As Integer, pDeviceGUID As IntPtr) As Integer
    End Function

    ' timeswitch
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_TimeSwitch", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_TimeSwitch(nSendType As Integer, pStrParams As IntPtr, nUse As Integer, nBootuUpTime As Integer, nShutDownTime As Integer, pDeviceGUID As IntPtr) As Integer
    End Function

    ' switch program
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_SwitchProgram", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_SwitchProgram(nSendType As Integer, pStrParams As IntPtr, nCtrlCode As Integer, pDeviceGUID As IntPtr) As Integer
    End Function

    ' Set ip
    <DllImport("HDSDK.dll", EntryPoint:="Cmd_SetIP", CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function Cmd_SetIP(pSrcIP As IntPtr, pDestIP As IntPtr, pDestMask As IntPtr, pDestGateway As IntPtr, pDeviceGUID As IntPtr) As Integer
    End Function

End Class
