#pragma warning (disable : 4005) // disables the irretating macro redefinition warning
# ifndef WIN32_LEAN_AND_MEAN
	# define WIN32_LEAN_AND_MEAN
#endif

#ifndef _WINDOWS_
	# include <windows.h>
#endif

#ifndef ushort
	#define ushort unsigned short
#endif

#ifndef uint
	#define uint unsigned int
#endif

#define LoadLibrary  LoadLibraryA

typedef DWORD (CALLBACK* LPFNDLLFUNC1)(LPCWSTR lpFileName);
typedef LPSTR (CALLBACK* LPFNDLLFUNC2)(LPCWSTR lpFileName);
typedef DWORD (CALLBACK* LPFNDLLFUNC3)(LPCWSTR lpString);
HINSTANCE hDLL;              
LPFNDLLFUNC1 CalculateCRC32;
LPFNDLLFUNC2 CalculateMD5;
LPFNDLLFUNC3 CalculateCRC32String;



inline void InitCRC32Table(PDWORD lpCRC32Table)
{
    register DWORD iPolynomial;
    register DWORD iCrc;
    DWORD i, j;

    iPolynomial = 0xEDB88320;
 
    for (i = 0 ; i<256; i++)
	{
        iCrc = i;
        for (j = 8; j>0; j--)
		{
            if (iCrc & 1)
			{
                iCrc = ((iCrc & 0xFFFFFFFE) >> 1) & 0x7FFFFFFF;
                iCrc = iCrc ^ iPolynomial;
			}
            else
                iCrc = ((iCrc & 0xFFFFFFFE) >> 1) & 0x7FFFFFFF;
		}
        lpCRC32Table[i] = iCrc;
	}
}