#include "stdafx.h"
#include "CRC32Tester.h"
#include <stdlib.h>

bool initDLL()
{
#ifdef _M_X64
	hDLL = LoadLibrary("CRC32x64.dll");
#else
	hDLL = LoadLibrary("CRC32.dll");
#endif	
	if (hDLL==NULL)
		return false;

	CalculateCRC32 = (LPFNDLLFUNC1)GetProcAddress(hDLL,"CalculateCRC32");
	CalculateCRC32String = (LPFNDLLFUNC3)GetProcAddress(hDLL,"GetCRC32FromString");
	CalculateMD5 = (LPFNDLLFUNC2)GetProcAddress(hDLL,"CalculateMD5");
	return true;
}
int DWORDToString(char* lpzDestination, int iStrPos, PDWORD lpDWORD)
{
	char *lpzTemp = new char[10];
	int iStrLength=0;
	ULONG ulTemp = (ULONG)*lpDWORD;
	_ultoa(ulTemp,lpzTemp, 10);
	
	for (int i=iStrPos; i<(iStrPos+11); i++)
	{
		*(lpzDestination+i) = lpzTemp[iStrLength];
		iStrLength++;
		if (lpzTemp[iStrLength]==NULL)
			break;
	}
	return iStrLength;
}

void strToFile(char* lpzFileName, UINT strLength, char* lpzStrData)
{
	FILE *fHandle = fopen(lpzFileName, "w");
	fwrite(lpzStrData, sizeof(char),strLength,fHandle);
	fclose(fHandle);
}
int _tmain(int argc, _TCHAR* argv[])
{
	if (!initDLL())
		return 1;
	int iStrPos = 14;

	DWORD crc32Result = CalculateCRC32(L"c:\\Movies\\Getaway In Luleå [Swe] [MC].avi"); //c:\\Blandat\\Halo3_e32006_announce_large.wmv");
	LPSTR md5Result = CalculateMD5(L"d:\\Movies\\300.2006.720p.BRRip.x264.DTS-anoXmous\\300.2006.720p.BRRip.x264.DTS-anoXmous.mkv");

	PDWORD CRC32Table = new DWORD[256];
	char* lpzCrc32Table= (char*)malloc(sizeof(char) * 10240);
	InitCRC32Table(CRC32Table);
	
	strcpy(lpzCrc32Table,"CRC32Table = {");
	for (int i=0; i<256; i++)
	{
		iStrPos += DWORDToString(lpzCrc32Table, iStrPos, &CRC32Table[i]);
		*(lpzCrc32Table+iStrPos) = ',';		
		iStrPos++;
	}
	*(lpzCrc32Table+iStrPos) = '}';
	iStrPos++;
	*(lpzCrc32Table+iStrPos) = 0;
		
	printf(lpzCrc32Table);
	
	const WCHAR* testStr = L"D:\\Musik\\VA - Future Trance Series\\VA - Future Trance Vol. 25\\121 - E Nomine - Das Omen (Im Kreis Des Bösen).mp32007-07-21 23:40:522007-07-22 14:17:11CRC32Node";
	DWORD result = CalculateCRC32String(testStr);

	char* lpzResult = (char*)malloc(sizeof(char) * 9);

	lpzResult = ultoa(result,lpzResult,10);

	printf(lpzResult);
	//strToFile("c:\\crc32table.txt",iStrPos+1,lpzCrc32Table);
	scanf(lpzCrc32Table);

	free(lpzCrc32Table);
	free(lpzResult);
	delete md5Result;
	return 0;
}


