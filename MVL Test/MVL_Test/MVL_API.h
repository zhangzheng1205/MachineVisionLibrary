#ifndef _MVL_API_IMPORT_LIBRARY
#define _MVL_API_IMPORT_LIBRARY

#include "windows.h"
#include "MVL.h"

typedef bool			 (*MVL_Create_Func)(char* pString);
typedef bool			 (*MVL_Destroy_Func)();
typedef MVL_RETURN_VALUE (*MVL_SetProperty_Func)(int nParameterId, void* pValue, void* pValue2, void* pValue3);
typedef MVL_RETURN_VALUE (*MVL_GetProperty_Func)(int nParameterId, void* pValue, void* pValue2, void* pValue3);
typedef MVL_RETURN_VALUE (*MVL_Analyze_Func)();
typedef MVL_ERROR_CODE	 (*MVL_GetLastError_Func)();

typedef enum _MVL_API_PROPERTY
{
	MVL_API_CREATE	= 0,
	MVL_API_CREATE_READ,
	MVL_API_DESTROY,
	MVL_API_DESTROY_READ,
	MVL_API_SET_PROPERTY,
	MVL_API_GET_PROPERTY,
	MVL_API_ANALYZE,
	MVL_API_GET_LAST_ERROR,

} MVL_API_PROPERTY;

class MVL_API
{
public:

	MVL_API();
	bool Initialize(char* pcDllPath);
	bool SetProperty(int nPropertyID, void* pValue, void* pValue2, void* pValue3);
	bool GetProperty(int nPropertyID, void* pValue, void* pValue2, void* pValue3);
	bool CallMethod(int nPropertyID, void* pValue, void* pValue2, void* pValue3);

private:

	HMODULE m_hDllLibrary;
	char* m_pDllPath;

	MVL_Create_Func m_Create;
	MVL_Destroy_Func m_Destroy;
	MVL_SetProperty_Func m_SetProperty;
	MVL_GetProperty_Func m_GetProperty;
	MVL_Analyze_Func m_Analyze;
	MVL_GetLastError_Func m_GetLastError;

	char m_pMVL_Create[20];
	char m_pMVL_Destroy[20];	
	char m_pMVL_SetProperty[20];	
	char m_pMVL_GetProperty[20];	
	char m_pMVL_Analyze[20];	
	char m_pMVL_GetLastError[20];
};

#endif //_MVL_API_IMPORT_LIBRARY