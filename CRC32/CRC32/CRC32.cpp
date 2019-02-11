#include "CRC32.h"
#include "MD5wrapper.h"
#include <iostream>
using namespace std;

#ifdef _MANAGED
#pragma managed(push, off)
#endif

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved )
{
    return TRUE;
}

ULONG APIENTRY GetCRC32FromString(LPCWSTR lpStringToHash)
{
	register DWORD crc32Result;
    register DWORD i;
    register DWORD iLookup;
	unsigned long crc32ResultCpy;
	SIZE_T strLength;
	PDWORD lpCRC32Table = NULL;

	#ifndef USE_CONST_CRC_TABLE
	lpCRC32Table = (PDWORD)malloc(sizeof(PDWORD)*256);
	InitCRC32Table(lpCRC32Table);
	#else
	lpCRC32Table = dwCRC32Table;
	#endif

	crc32Result = 0xFFFFFFFF;
	strLength = wcslen(lpStringToHash);

	for (i=0; i<strLength; i++)
	{
		iLookup = (crc32Result & 0xFF) ^ lpStringToHash[i];
		crc32Result = ((crc32Result & 0xFFFFFF00) >> 8) & 0xFFFFFF;
		crc32Result = crc32Result ^ lpCRC32Table[iLookup];
	}
	crc32ResultCpy = ~crc32Result;
	return crc32ResultCpy;
}
ULONG APIENTRY CalculateCRC32(LPWSTR lpFileName)
{
	register DWORD crc32Result;
    register DWORD i;
    register DWORD iLookup;
	DWORD dwBufferSize;
	DWORD dwBytesRead = 0;
	DWORD iCnt;
	BYTE *lpstrFileBuffer;	
	HANDLE fHandle;
	DWORD dwFileSizeLow, dwFileSizeHigh;
	PDWORD lpCRC32Table=0;

	fHandle = CreateFile(
        lpFileName,
        FILE_READ_DATA|FILE_READ_ATTRIBUTES,
        FILE_SHARE_READ,
        NULL,
        OPEN_EXISTING,
        0,
        NULL);
	
	if (fHandle==INVALID_HANDLE_VALUE)
		return 0;

	dwFileSizeLow = GetFileSize(fHandle, &dwFileSizeHigh);	
	
	//Calculate buffer size
	if (dwFileSizeLow>4)
		dwBufferSize = dwFileSizeLow/4;	
	else
		dwBufferSize = dwFileSizeLow;

	if (dwBufferSize==0)
	{
		CloseHandle(fHandle);
		return 0;
	}

	if (dwBufferSize > MAX_BUFFER_SIZE)
		dwBufferSize = MAX_BUFFER_SIZE;
	else
	{
		if ((dwBufferSize%2)>0)
			dwBufferSize++;
		iCnt = 0;
		while (dwBufferSize!=1)
		{
			iCnt++;
			dwBufferSize = dwBufferSize>>1;
		}
		dwBufferSize = 0x1<<iCnt;
	}	
	
	lpstrFileBuffer = (BYTE*) malloc(dwBufferSize * sizeof(BYTE));
	
	
	#ifndef USE_CONST_CRC_TABLE
	lpCRC32Table = (PDWORD)malloc(sizeof(PDWORD)*256);
	InitCRC32Table(lpCRC32Table);
	#else
	lpCRC32Table = dwCRC32Table;
	#endif
	crc32Result = 0xFFFFFFFF;

	do 
	{
		ReadFile(fHandle, lpstrFileBuffer, dwBufferSize, &dwBytesRead, NULL);		
		for (i=0; i<dwBytesRead; i++)
		{
			iLookup = (crc32Result & 0xFF) ^ lpstrFileBuffer[i];
			crc32Result = ((crc32Result & 0xFFFFFF00) >> 8) & 0xFFFFFF;
			crc32Result = crc32Result ^ lpCRC32Table[iLookup];
		}
	}while (dwBytesRead>0);

	CloseHandle(fHandle);
	free(lpstrFileBuffer);

	#ifndef USE_CONST_CRC_TABLE
	free(lpCRC32Table);
	#endif

	return ~crc32Result;
}

BOOL APIENTRY CalculateMD5(LPWSTR lpFileName, LPSTR lpMD5)
{
	md5wrapper *oMD5 = new md5wrapper();
	int nSize;
	LPSTR lpszName = new char[257];		
	
	nSize = ::WideCharToMultiByte(CP_ACP, 0, lpFileName, -1, lpszName, 257, NULL, NULL);

	string sFileName = string(lpszName);
	string sMD5 = oMD5->getHashFromFile(sFileName);	
	
	if(IsBadWritePtr(&lpMD5, sMD5.length()))
    {
        SetLastError(ERROR_INSUFFICIENT_BUFFER);
        return FALSE;
    }

	strcpy(lpMD5, sMD5.c_str());

	delete oMD5;
	delete lpszName;	

	return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif

