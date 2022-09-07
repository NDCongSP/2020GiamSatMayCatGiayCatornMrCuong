/********************************************************************
	��������:	2016:1:28  11:01
	����:		HUIDU
	˵��:		���ο�������������ͷ�ļ���
*********************************************************************/
#ifndef _HDEXPORT_H
#define  _HDEXPORT_H

#define HDAPI _stdcall


extern "C"
{
	// ������һ�κ������ô������
	int HDAPI Hd_GetSDKLastError();

	// ������Ļ
	int HDAPI Hd_CreateScreen(int nWidth, int nHeight, int nColor, int nGray, int nCardType, void *pExParamsBuf, int nBufSize);

	// ������Ŀ
	int HDAPI Hd_AddProgram( void *pBoderImgPath, int nBorderEffect, int nBorderSpeed, void *pExParamsBuf, int nBufSize);

	// ��������
	int HDAPI Hd_AddArea(int nProgramID, int nX, int nY, int nWidth, int nHeight, void *pBoderImgPath,  int nBorderEffect, int nBorderSpeed, void *pExParamsBuf, int nBufSize);
	
	// ����ͼƬ������
	int HDAPI Hd_AddImageAreaItem(int nAreaID, void *pPaths, int nShowEffect, int nShowSpeed, int nClearType, int nStayTime, void *pExParamsBuf, int nBufSize);

	// ��Ӽ��ı�������
	int HDAPI Hd_AddSimpleTextAreaItem(int nAreaID, void *pText, int nTextColor, int nBackGroupColor, int nStyle, void *pFontName, int nFontHeight,  int nShowEffect, int nShowSpeed, int nClearType, int nStayTime, void *pExParamsBuf, int nBufSize);

	// ���ʱ��������
	int HDAPI Hd_AddTimeAreaItem(int nAreaID, int nShowMode, int bShowDate, int nDateStyle,  int bShowWeek, int nWeekStyle, int bShowTime, int nTimeStyle, int nTextColor, void *pFontName, int nFontHeight, int nDiffHour, int nDiffMin, void *pExParamsBuf, int nBufSize);

	// ����ʵʱ����
	int HDAPI Hd_CreateRealtimeArea(int nX, int nY, int nWidth, int nHeight, void *pImgPath, int nUseRealTime,  int bOnlyShowRealtimeArea,  int bSave, int nLivetime, void *pExParamsBuf, int nBufSize);

	// ������������
	int HDAPI Hd_SendCommand(int nSendType, void *pStrParams, int nCommandType, void *pConText, int nTextLen, void *pDeviceGUID, void *pOutConText, int *pOutConTextLen, void *pExParamsBuf, int nBufSize);

	// ������Ļ����
	int HDAPI Hd_SendScreen(int nSendType, void *pStrParams, void *pDeviceGUID, void *pExParamsBuf, int nBufSize);

	// ����ʵʱ��������
	int HDAPI Hd_SendRealTimeArea(int nSendType, void *pStrParams, void *pDeviceGUID, void *pExParamsBuf, int nBufSize);

	// �����ɫ
	int HDAPI Hd_GetColor(int r, int g, int b);

	// ������Ļ������ݡ���Ļ��Ϣ���ݵ�ָ��Ŀ¼
	int HDAPI Hd_SaveScreen(void *pDirPath);

	////////////////////////////////////////////////////////////////////////////
	// �������
	// ��ô����豸������
	int HDAPI Cmd_GetBaudRate(int nPort, int *pBaudRate, void *pDeviceGUID);

	// ���ô����豸������
	int HDAPI Cmd_SetBaudRate(int nPort, int nSrcBaudRate, int nDestBaudRate, void *pDeviceGUID);

	// ���Կ��ƿ��Ƿ�����
	int HDAPI Cmd_IsCardOnline(int nSendType, void *pStrParams, void *pDeviceGUID);

	// �����ʾ������
	int HDAPI Cmd_ClearScreen(int nSendType, void *pStrParams, void *pDeviceGUID);

	// �������ƿ�
	int HDAPI Cmd_RestartCard(int nSendType, void *pStrParams, void *pDeviceGUID);

	// ������ʾ��
	int HDAPI Cmd_TestScreen(int nSendType, void *pStrParams, int nStyle, void *pDeviceGUID);

	// У׼ʱ��
	int HDAPI Cmd_AdjustTime(int nSendType, void *pStrParams, void *pDeviceGUID);

	// ��������
	int HDAPI Cmd_SetLuminance(int nSendType, void *pStrParams, int nLuminance, void *pDeviceGUID);

	// ��Ļ���� �������� ���� ��ͣ
	int HDAPI Cmd_ScreenCtrl(int nSendType, void *pStrParams, int nCtrlCode, void *pDeviceGUID);

	// ��ʱ���ػ�
	int HDAPI Cmd_TimeSwitch(int nSendType, void *pStrParams, int nUse, int nBootuUpTime, int nShutDownTime, void *pDeviceGUID);

	// ��Ŀ�л�
	int HDAPI Cmd_SwitchProgram(int nSendType, void *pStrParams, int nProgramIndex, void *pDeviceGUID);

	// �����豸IP
	int HDAPI Cmd_SetIP(void *pSrcIP, void *pDestIP, void *pDestMask, void *pDestGateway, void *pDeviceGUID);

	//////////////////////////////////////////////////////////////////////////////
	// �߼����������������Լ�ʵ��ͨѸʱʹ��

	// ׼����Ļ���Ͱ�������ܰ���
	int HDAPI Ad_PrepareScreenData(int *pTotalPacks, void *pDeviceGUID, void *pExParamsBuf, int nBufSize);

	// �����Ļ���Ͱ�
	int HDAPI Ad_GetScreenSendPack(int nPackIndex, void *pOut, int *pLen);

	// ׼��ʵʱ���������ݰ��������ܰ���
	int HDAPI Ad_PrepareRealtimeAreaData(int *pTotalPacks, void *pDeviceGUID,  void *pExParamsBuf, int nBufSize);

	// ���ʵʱ�����Ͱ�
	int HDAPI Ad_GetRealtimeAreaSendPack(int nPackIndex, void *pOut, int *pLen);

	// ���ͨ���������ݰ�
	int HDAPI Ad_GetCommandPack(int nCommandType, void *pConText, int nTextLen, void *pOut, int *pnLen, void *pDeviceGUID, void *pExParamsBuf, int nBufSize);

	// �������ذ�
	int HDAPI Ad_ResolveReturnPack(void *pSrcData, int nSrcLen, void *pOutConText, int *pOutConTextLen);

	//////////////////////////////////////////////////////////////////////////////
};
#endif