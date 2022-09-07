// HDSDK_Demo_C.cpp : Defines the entry point for the console application.
//

#include <cstdio>
#include "HDExport.h"
#pragma comment(lib, "HDSDK.lib")

int Demo1_SendText();	
int Demo2_SendImage();
int Demo3_RealtimeArea();
int Demo4_SendCmd();
int Demo5_SendTime();

using namespace std;


int main()
{
	//std::bind
	// 1.Send text
	 Demo1_SendText();

	// 2.Send image
	// Demo2_SendImage();

	// 3.Send RealtimeArea
	// Demo3_RealtimeArea();

	// 4.Send Command
	//Demo4_SendCmd();

	 // 5. Send time
	 //Demo5_SendTime();
	return 0;
}
#include <string>

int Demo1_SendText()
{
	int nError = 0;
	// 1. Create a screen
	int nWidth = 64;
	int nHeight = 32;
	int nColor = 1;
	int nGray = 1;
	int nCardType = 0;

	int nRe = Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, 0, 0);
	if (nRe != 0)
	{
		printf("SetScreenParams Error! \n");
		return -1;
	}

	// 2. Add program to screen
	int nProgramID = Hd_AddProgram(0, 0, 5, 0, 0);
	if (nProgramID == -1)
	{
		nError = Hd_GetSDKLastError();
		return -1;
	}

	int nX = 0;
	int nY = 0;
	int nAreaWidth = 64;
	int nAreaHeight = 32;

	// 3. Add Area to program
	int nAreaID = Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, 0, 0, 5, 0, 0);
	if (nAreaID == -1)
	{
		nError = Hd_GetSDKLastError();
		return -1;
	}

	// 4.Add text AreaItem to Area

	wchar_t szText[256] = {0};
	swprintf(szText, 256, L"OK%d", 1);
	int nFontColor = Hd_GetColor(255,0,0);
	int nAreaItemID = Hd_AddSimpleTextAreaItem(nAreaID, szText, nFontColor, 0, 4|0x0100|0x0200, L"Arial", 24, 0, 25, 201, 3, 0, 0);
	if (nAreaItemID == -1)
	{
		nError = Hd_GetSDKLastError();
		return -1;
	}

	// Save Screen File
	//nRe = Hd_SaveScreen(L"D:\\");
	//if(nRe !=0 )
	//{
	//	nError = Hd_GetSDKLastError();
	//}

	bool bSendTcp = true;
	if (bSendTcp)
	{
		// Send to device use tcp
		nRe = Hd_SendScreen(0, L"192.168.2.119", 0, 0, 0);
		if (nRe == -1)
		{
			nError = Hd_GetSDKLastError();
		}
	} 
	else
	{
		// Send to device use serial
		wchar_t szParams[256] = {0};
		int nPort = 4;
		int nSeariaBaudRate = 57600;
		swprintf(szParams, 256, L"%d:%d", nPort, nSeariaBaudRate);
		nRe = Hd_SendScreen(1, szParams, 0, 0, 0);
		if (nRe == -1)
		{
			nError = Hd_GetSDKLastError();
		}
	}

	return 0;
}

int Demo2_SendImage()
{
	// 1. Create a screen
	int nWidth = 64;
	int nHeight = 32;
	int nColor = 1;
	int nGray = 1;
	int nCardType = 0;

	int nRe = Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, 0, 0);
	if (nRe != 0)
	{
		printf("SetScreenParams Error! \n");
		return -1;
	}

	// 2. Add program to screen
	int nProgramID = Hd_AddProgram(0, 0, 5, 0, 0);
	if (nProgramID == -1)
	{
		int nError = Hd_GetSDKLastError();
		return -1;
	}

	int nX = 0;
	int nY = 0;
	int nAreaWidth = 64;
	int nAreaHeight = 32;

	// 3. Add Area to program
	int nAreaID = Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, 0, 0, 5, 0, 0);
	if (nAreaID == -1)
	{
		int nError = Hd_GetSDKLastError();
		return -1;
	}


	// 4.Add Image AreaItem to Area
	wchar_t path[] = L"d:\\test.bmp\r\nd:\\test2.bmp\r\nd:\\test3.bmp";
	int nShowEffect = 0;
	int nShowSpeed = 30;
	int nClearType = 201;
	int nStayTime = 3;
	int nAreaItemID = Hd_AddImageAreaItem(nAreaID, path, nShowEffect, nShowSpeed, nClearType, nStayTime, 0, 0);

	// 5. Send to device use tcp
	int nError = Hd_SendScreen(0, L"192.168.2.119", 0, 0, 0);

	// Send to device use serial
	//wchar_t szParams[256] = {0};
	//int nPort = 4;
	//int nSeariaBaudRate = 57600;
	//swprintf(szParams, 256, L"%d:%d", nPort, nSeariaBaudRate);
	//int nError = Hd_SendScreen(1, szParams, 0, 0, 0);

	return 0;
}

int Demo3_RealtimeArea()
{
	// 1. Create a screen
	int nWidth = 64;
	int nHeight = 32;
	int nColor = 1;
	int nGray = 1;
	int nCardType = 0;

	int nRe = Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, 0, 0);
	if (nRe != 0)
	{
		printf("SetScreenParams Error! \n");
		return -1;
	}

	// CreateRealtimeArea
	int nX = 0;
	int nY = 16;
	int nAreaWidth = 64;
	int nAreaHeight = 16;
	nRe = Hd_CreateRealtimeArea(nX, nY, nAreaWidth, nAreaHeight, L"d:\\test.bmp", 1, 0, 0, 0, 0, 0);
	if (nRe != 0)
	{
		return -1;
	}

	nRe = Hd_SendRealTimeArea(0, L"192.168.2.201", 0, 0, 0);
	if (nRe != 0)
	{
		return -1;
	}

	// Send to device use serial
	//wchar_t szParams[256] = {0};
	//int nPort = 4;
	//int nSeariaBaudRate = 57600;
	//swprintf(szParams, 256, L"%d:%d", nPort, nSeariaBaudRate);
	//int nError = Hd_SendSRealTimeArea(1, szParams, 0, 0, 0);

	return 0;
}

int Demo4_SendCmd()
{

	int nRe = 0;

	int nSendType = 0;  // 0 tcp 1 serial port
	wchar_t *pStrParams = 0;
	if (nSendType == 0)
	{
		pStrParams =  L"192.168.2.201";
	}
	else
	{
		pStrParams = L"4:57600";
	}

	// 1 check is card online.
	nRe = Cmd_IsCardOnline(nSendType, pStrParams, 0);

	// 2 clear screen
	//nRe = Cmd_ClearScreen(nSendType, pStrParams, 0);

	// Resend text
	// Demo1_SendText();

	// 3 restart card
	//nRe =Cmd_RestartCard(nSendType, pStrParams, 0);

	// 4 test screen
	//nRe =Cmd_TestScreen(nSendType, pStrParams, 0, 0);

	// Close test screen
	// nRe = Cmd_TestScreen(nSendType, pStrParams, 0xff, 0);

	// 5.AdjustTime
	//nRe = Cmd_AdjustTime(nSendType, pStrParams, 0);

	// 6.SetLuminance
	//nRe = Cmd_SetLuminance(nSendType, pStrParams, 49, 0);

	// 7.Cmd_ScreenCtrl
	//nRe = Cmd_ScreenCtrl(nSendType, pStrParams, 5, 0);

	//nRe = Cmd_ScreenCtrl(nSendType, pStrParams, 6, 0);

	// 8.Cmd_TimeSwitch
	//nRe = Cmd_TimeSwitch(nSendType, pStrParams, 1, 6 * 60 * 60, 21 * 60 * 60, 0);

	// 9.Cmd_SwitchProgram
	// show program1
	//nRe = Cmd_SwitchProgram(nSendType, pStrParams, 1, 0);

	// Restore
	//nRe = Cmd_SwitchProgram(nSendType, pStrParams, 0, 0);

	// 10.Cmd_SetIP
	nRe = Cmd_SetIP(L"192.168.2.201", L"192.168.2.203", L"255.255.255.0", L"192.168.2.1", 0);

	//// Get and Set baudrate
	//int nRate = 0;
	//nRe = Cmd_GetBaudRate(4, &nRate, 0, 0);
	//if (nRe == 0)
	//{
	//	nRe = Cmd_SetBaudRate(4, nRate, 57600, 0);
	//}

	return nRe;
}

int Demo5_SendTime()
{
	// 1. Create a screen
	int nWidth = 64;
	int nHeight = 32;
	int nColor = 1;
	int nGray = 1;
	int nCardType = 0;

	int nRe = Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, 0, 0);
	if (nRe != 0)
	{
		printf("SetScreenParams Error! \n");
		return -1;
	}

	// 2. Add program to screen
	int nProgramID = Hd_AddProgram(0, 0, 5, 0, 0);
	if (nProgramID == -1)
	{
		int nError = Hd_GetSDKLastError();
		return -1;
	}

	int nX = 0;
	int nY = 0;
	int nAreaWidth = 64;
	int nAreaHeight = 32;

	// 3. Add Area to program
	int nAreaID = Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, 0, 0, 5, 0, 0);
	if (nAreaID == -1)
	{
		int nError = Hd_GetSDKLastError();
		return -1;
	}


	// 4.Add time AreaItem to Area

	int nFontColor = Hd_GetColor(0,255,0);
	int nAreaItemID = Hd_AddTimeAreaItem(nAreaID, 0, 1, 0, 0, 0, 0, 0, nFontColor, L"Arail", 16, 0, 0, 0, 0);

	// 5. Send to device use tcp
	//int nError = Hd_SendScreen(0, L"192.168.2.201", 0, 0, 0);

	// Send to device use serial
	wchar_t szParams[256] = {0};
	int nPort = 4;
	int nSeariaBaudRate = 57600;
	swprintf(szParams, 256, L"%d:%d", nPort, nSeariaBaudRate);
	int nError = Hd_SendScreen(1, szParams, 0, 0, 0);

	return 0;
}