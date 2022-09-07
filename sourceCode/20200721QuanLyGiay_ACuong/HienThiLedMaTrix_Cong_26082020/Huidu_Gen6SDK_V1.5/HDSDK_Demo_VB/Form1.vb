
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices



Public Class Form1

    Sub InitializeSendParams()
        'init send type
        Dim bSendUseIp As Boolean = True

        Dim strSendParam As String = "192.168.2.201"

        If (bSendUseIp) Then
            m_nSendType = 0
            Dim strParams As String = "192.168.2.201"
            m_pSendParams = Marshal.StringToHGlobalUni(strParams)
        Else

            'Serial port see Cmd_GetBaudRate Cmd_SetBaudRate
            Dim nSerialPort As Integer = 4
            Dim nBaudRate As Integer = 57600
            Dim strParams As String = nSerialPort.ToString() + ":" + nBaudRate.ToString()

            m_nSendType = 1
            m_pSendParams = Marshal.StringToHGlobalUni(strParams)
        End If
    End Sub

    Private Sub ButtonText_Click(sender As System.Object, e As System.EventArgs) Handles ButtonText.Click

        Dim pNULL As IntPtr = 0
        Dim nErrorCode As Integer = 0

        ' 1. Create a screen
        Dim nWidth As Integer = 64
        Dim nHeight As Integer = 32
        Dim nColor As Integer = 1
        Dim nGray As Integer = 1
        Dim nCardType As Integer = 0

        nErrorCode = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If


        '// 2. Add program to screen
        Dim nProgramID As Integer = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0)
        If (nProgramID = -1) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        '// 3. Add Area to program
        Dim nX As Integer = 0
        Dim nY As Integer = 0
        Dim nAreaWidth As Integer = 64
        Dim nAreaHeight As Integer = 32
        Dim nAreaID As Integer = CSDKExport.Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0)
        If (nAreaID = -1) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If


        '// 4.Add text AreaItem to Area
        Dim pText As IntPtr = Marshal.StringToHGlobalUni("VB123")
        Dim pFontName As IntPtr = Marshal.StringToHGlobalUni("Arial")
        Dim nTextColor As Integer = CSDKExport.Hd_GetColor(255, 0, 0)
        Dim nAreaItemID As Integer = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID, pText, nTextColor, 0, 4, pFontName, 16, 0, 30, 201, 3, pNULL, 0)
        If (nAreaItemID = -1) Then
            Marshal.FreeHGlobal(pText)
            Marshal.FreeHGlobal(pFontName)
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        Marshal.FreeHGlobal(pText)
        Marshal.FreeHGlobal(pFontName)

        '// 5. Send to device 
        nErrorCode = CSDKExport.Hd_SendScreen(m_nSendType, m_pSendParams, pNULL, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
        End If

    End Sub

    Private Sub ButtonImage_Click(sender As System.Object, e As System.EventArgs) Handles ButtonImage.Click

        Dim pNULL As IntPtr = 0
        Dim nErrorCode As Integer = 0

        ' 1. Create a screen
        Dim nWidth As Integer = 64
        Dim nHeight As Integer = 32
        Dim nColor As Integer = 1
        Dim nGray As Integer = 1
        Dim nCardType As Integer = 0

        nErrorCode = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If


        '// 2. Add program to screen
        Dim nProgramID As Integer = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0)
        If (nProgramID = -1) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        '// 3. Add Area to program
        Dim nX As Integer = 0
        Dim nY As Integer = 0
        Dim nAreaWidth As Integer = 64
        Dim nAreaHeight As Integer = 32
        Dim nAreaID As Integer = CSDKExport.Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0)
        If (nAreaID = -1) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If


        '// 4.Add text AreaItem to Area
        Dim pPath As IntPtr = Marshal.StringToHGlobalUni("d:\\test.bmp")
        Dim nAreaItemID As Integer = CSDKExport.Hd_AddImageAreaItem(nAreaID, pPath, 0, 30, 201, 3, pNULL, 0)
        If (nAreaItemID = -1) Then
            Marshal.FreeHGlobal(pPath)
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        Marshal.FreeHGlobal(pPath)

        '// 5. Send to device 
        nErrorCode = CSDKExport.Hd_SendScreen(m_nSendType, m_pSendParams, pNULL, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
        End If

    End Sub

    Private Sub ButtonTime_Click(sender As System.Object, e As System.EventArgs) Handles ButtonTime.Click

        Dim pNULL As IntPtr = 0
        Dim nErrorCode As Integer = 0

        ' 1. Create a screen
        Dim nWidth As Integer = 64
        Dim nHeight As Integer = 32
        Dim nColor As Integer = 1
        Dim nGray As Integer = 1
        Dim nCardType As Integer = 0

        nErrorCode = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        '// 2. Add program to screen
        Dim nProgramID As Integer = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0)
        If (nProgramID = -1) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        '// 3. Add Area to program
        Dim nX As Integer = 0
        Dim nY As Integer = 0
        Dim nAreaWidth As Integer = 64
        Dim nAreaHeight As Integer = 32
        Dim nAreaID As Integer = CSDKExport.Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0)
        If (nAreaID = -1) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If


        '// 4.Add text AreaItem to Area
        Dim pFontName As IntPtr = Marshal.StringToHGlobalUni("Arial")
        Dim nTextColor As Integer = CSDKExport.Hd_GetColor(255, 0, 0)
        Dim nAreaItemID As Integer = CSDKExport.Hd_AddTimeAreaItem(nAreaID, 0, 1, 0, 0, 0, 0, 0, nTextColor, pFontName, 16, 0, 0, pNULL, 0)
        If (nAreaItemID = -1) Then
            Marshal.FreeHGlobal(pFontName)
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        Marshal.FreeHGlobal(pFontName)

        '// 5. Send to device 
        nErrorCode = CSDKExport.Hd_SendScreen(m_nSendType, m_pSendParams, pNULL, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
        End If

    End Sub

    Private Sub ButtonRealTimeArea_Click(sender As System.Object, e As System.EventArgs) Handles ButtonRealTimeArea.Click

        Dim pNULL As IntPtr = 0
        Dim nErrorCode As Integer = 0

        ' 1. Create a screen
        Dim nWidth As Integer = 64
        Dim nHeight As Integer = 32
        Dim nColor As Integer = 1
        Dim nGray As Integer = 1
        Dim nCardType As Integer = 0

        nErrorCode = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
            Return
        End If

        ' 2. CreateRealtimeArea
        Dim nX As Integer = 0
        Dim nY As Integer = 0
        Dim nAreaWidth As Integer = 64
        Dim nAreaHeight As Integer = 16
        Dim pPath As Integer = Marshal.StringToHGlobalUni("d:\\test.bmp")

        nErrorCode = CSDKExport.Hd_CreateRealtimeArea(nX, nY, nAreaWidth, nAreaHeight, pPath, 1, 0, 0, 0, pNULL, 0)
        If (nErrorCode <> 0) Then
            Marshal.FreeHGlobal(pPath)
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
        End If
        Marshal.FreeHGlobal(pPath)

        '// 3. Send to device 
        nErrorCode = CSDKExport.Hd_SendRealTimeArea(m_nSendType, m_pSendParams, pNULL, pNULL, 0)
        If (nErrorCode <> 0) Then
            nErrorCode = CSDKExport.Hd_GetSDKLastError()
        End If

    End Sub

    Private Sub ButtonCmd_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCmd.Click

        Dim pNULL As IntPtr = 0
        Dim nErrorCode As Integer = 0

        '// 1 check is card online.
        nErrorCode = CSDKExport.Cmd_IsCardOnline(m_nSendType, m_pSendParams, pNULL)

        '// 2 clear screen
        nErrorCode = CSDKExport.Cmd_ClearScreen(m_nSendType, m_pSendParams, pNULL)


        '// Get and Set baudrate
        'Dim nRate As Integer = 0
        'nErrorCode = CSDKExport.Cmd_GetBaudRate(4, nRate, pNULL)
        'If (nErrorCode = 0) Then
        '    nErrorCode = CSDKExport.Cmd_SetBaudRate(4, nRate, 57600, pNULL)
        'End If

    End Sub
End Class
