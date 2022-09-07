// HDSDK_Demo_C_CustomCommunication.cpp 

//

#include "stdafx.h"
#include <Windows.h>
#include <WinSock.h>
#include "HDExport.h"
#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "HDSDK.lib")

int Demo1_SendText(); 
int Demo2_SendImage();
int Demo3_RealtimeArea();
int Demo4_SendCmd();
int Demo5_SendTime();
int Demo6_GetCommandPack();

int _tmain(int argc, _TCHAR* argv[])
{
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

	//6. GetCommandPadk
	//Demo6_GetCommandPack();
	return 0;
}

int Demo6_GetCommandPack()
{
	// Init Socket
	int nError = -1;
	WSADATA wsa;
	if (0 != ::WSAStartup(MAKEWORD(2, 2), &wsa))
	{
		return -1;
	}
	if (LOBYTE(wsa.wVersion) != 2 ||
		HIBYTE(wsa.wVersion) != 2 )
	{
		WSACleanup();
		return -1; 
	}

	// Create socket 
	SOCKET SockClient = socket(AF_INET, SOCK_STREAM, 0);
	if (INVALID_SOCKET == SockClient)
	{
		return -1; 
	}

	int nPort = 6101;
	char *pIP = "192.168.2.203";
	sockaddr_in ServerAddr;
	ServerAddr.sin_family = AF_INET;
	ServerAddr.sin_addr.S_un.S_addr = inet_addr(pIP);
	ServerAddr.sin_port = htons(nPort);

	int nTimeOut = 3000;
	::setsockopt(SockClient, SOL_SOCKET, SO_RCVTIMEO, (char *)&nTimeOut, sizeof(int));

	// connect device
	if (SOCKET_ERROR == connect(SockClient, (sockaddr *)&ServerAddr, sizeof(sockaddr)))
	{   
		closesocket(SockClient);
		return -1; 
	}

	//  Get Command Pack
	char buf[2048] = {0};
	char iBuf[2];
	iBuf[0] = 2; 
	iBuf[1] = 0;
	char OutContext[2048] = {0};
	int nOutLen = 0;
	nError =  ::Ad_GetCommandPack(0x12,iBuf,2,OutContext,&nOutLen,0,0,0);

	// send
	int nSend = send(SockClient, OutContext, nOutLen, 0);
	if (SOCKET_ERROR == nSend)
	{
		closesocket(SockClient);
		return - 1;
	}

	// recv and resolve return pack
	int nRecv = recv(SockClient, buf, 2048, 0);
	if (SOCKET_ERROR  != nRecv)
	{


	}

	closesocket(SockClient);

	WSACleanup();
	return 0;
}

int Demo3_RealtimeArea()
{
	int nError = -1;
	// 1. Create a screen
	int nWidth = 160;
	int nHeight = 128;
	int nColor = 2;
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
	int nAreaHeight = 32;
	nRe = Hd_CreateRealtimeArea(nX, nY, nAreaWidth, nAreaHeight, L"d:\\test.bmp", 1, 0, 0, 0, 0, 0);
	if (nRe != 0)
	{
		return -1;
	}

	// Send to device use tcp
	/*nRe = Hd_SendRealTimeArea(0, L"192.168.2.203", 0, 0, 0);
	if (nRe != 0)
	{
		return -1;
	}*/


	int nTotalPacks = 0;
	nError = Ad_PrepareRealtimeAreaData(&nTotalPacks, 0, 0, 0);

	if (nError != 0)
	{
		return -1;
	}

	WSADATA wsa;
	if (0 != ::WSAStartup(MAKEWORD(2, 2), &wsa))
	{
		return -1;
	}
	if (LOBYTE(wsa.wVersion) != 2 ||
		HIBYTE(wsa.wVersion) != 2 )
	{
		WSACleanup();
		return -1; 
	}

	// Create socket 
	SOCKET SockClient = socket(AF_INET, SOCK_STREAM, 0);
	if (INVALID_SOCKET == SockClient)
	{
		return -1; 
	}

	int nPort = 6101;
	char *pIP = "192.168.2.203";
	sockaddr_in ServerAddr;
	ServerAddr.sin_family = AF_INET;
	ServerAddr.sin_addr.S_un.S_addr = inet_addr(pIP);
	ServerAddr.sin_port = htons(nPort);

	int nTimeOut = 3000;
	::setsockopt(SockClient, SOL_SOCKET, SO_RCVTIMEO, (char *)&nTimeOut, sizeof(int));

	// connect device
	if (SOCKET_ERROR == connect(SockClient, (sockaddr *)&ServerAddr, sizeof(sockaddr)))
	{   
		closesocket(SockClient);
		return -1; 
	}


	// send every pack
	for (int n = 0; n < nTotalPacks; ++n)
	{
		char buf[2048] = {0};
		int nLen = 2048;
		nError = Ad_GetRealtimeAreaSendPack(n, buf, &nLen);
		if (nError != 0)
		{
			break;
		}

		int nSend = send(SockClient, buf, nLen, 0);
		if (SOCKET_ERROR == nSend)
		{
			closesocket(SockClient);
			return - 1;
		}

		// recv and resolve return pack
		int nRecv = recv(SockClient, buf, 2048, 0);
		if (SOCKET_ERROR  != nRecv)
		{
			char OutContext[2048] = {0};
			int nOutLen = 0;
			nError =  ::Ad_GetCommandPack(0, 0,0, OutContext, &nOutLen,0,0,0);
			if (nError != 0)
			{

				break;
			}
		}
		else
		{
			break;
		}
	}
	closesocket(SockClient);

	WSACleanup();

	return 0;
}



int Demo1_SendText()
{
	int nError = -1;
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

	//wchar_t szText[256] = {0};
	//swprintf(szText, 256,  L"222OK%d", 2);
	wchar_t *szText = L"123456789455hsfasfhelyouname1235";
	int nFontColor = Hd_GetColor(255,0,0);
	int nAreaItemID = Hd_AddSimpleTextAreaItem(nAreaID, szText, nFontColor, Hd_GetColor(0,0,0), 4, L"Arial", 24, 204, 60, 201, 3, 0, 0);
	if (nAreaItemID == -1)
	{
		nError = Hd_GetSDKLastError();
		return -1;
	}

	// 5. Send to device use tcp
	//nError = SendScreen(0, L"192.168.2.201", 0, 0, 0);

	int nTotalPacks = 0;
	nError = Ad_PrepareScreenData(&nTotalPacks, 0, 0, 0);

	if (nError != 0)
	{
		return -1;
	}

	WSADATA wsa;
	if (0 != ::WSAStartup(MAKEWORD(2, 2), &wsa))
	{
		return -1;
	}
	if (LOBYTE(wsa.wVersion) != 2 ||
		HIBYTE(wsa.wVersion) != 2 )
	{
		WSACleanup();
		return -1; 
	}

	// Create socket 
	SOCKET SockClient = socket(AF_INET, SOCK_STREAM, 0);
	if (INVALID_SOCKET == SockClient)
	{
		return -1; 
	}

	int nPort = 6101;
	char *pIP = "192.168.2.203";
	sockaddr_in ServerAddr;
	ServerAddr.sin_family = AF_INET;
	ServerAddr.sin_addr.S_un.S_addr = inet_addr(pIP);
	ServerAddr.sin_port = htons(nPort);

	int nTimeOut = 3000;
	::setsockopt(SockClient, SOL_SOCKET, SO_RCVTIMEO, (char *)&nTimeOut, sizeof(int));

	// connect device
	if (SOCKET_ERROR == connect(SockClient, (sockaddr *)&ServerAddr, sizeof(sockaddr)))
	{   
		closesocket(SockClient);
		return -1; 
	}


	// send every pack
	for (int n = 0; n < nTotalPacks; ++n)
	{
		char buf[2048] = {0};
		int nLen = 2048;
		nError = Ad_GetScreenSendPack(n, buf, &nLen);
		if (nError != 0)
		{
			break;
		}

		int nSend = send(SockClient, buf, nLen, 0);
		if (SOCKET_ERROR == nSend)
		{
			closesocket(SockClient);
			return - 1;
		}

		// recv and resolve return pack
		int nRecv = recv(SockClient, buf, 2048, 0);
		if (SOCKET_ERROR  != nRecv)
		{
			char OutContext[2048] = {0};
			int nOutLen = 0;
			nError =  ::Ad_ResolveReturnPack(buf, nRecv, OutContext, &nOutLen);
			if (nError != 0)
			{

				break;
			}
		}
		else
		{
			break;
		}
	}
	closesocket(SockClient);

	WSACleanup();
	return 0;
}