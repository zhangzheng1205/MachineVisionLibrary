#include "MVL_API.h"
#include "stdio.h"
#include "string.h"
#include "windows.h"

MVL_API::MVL_API()
{
	m_hDllLibrary = NULL;
	m_pDllPath = NULL;

	m_Create = NULL;
	m_Destroy = NULL;
	m_SetProperty = NULL;
	m_GetProperty = NULL;
	m_Analyze = NULL;
	m_GetLastError = NULL;

	sprintf(m_pMVL_Create, "MVL_Create");
	sprintf(m_pMVL_Destroy, "MVL_Destroy");
	sprintf(m_pMVL_SetProperty, "MVL_SetProperty");
	sprintf(m_pMVL_GetProperty, "MVL_GetProperty");
	sprintf(m_pMVL_Analyze, "MVL_Analyze");
	sprintf(m_pMVL_GetLastError, "MVL_GetLastError");
}

bool MVL_API::Initialize(char* pcDllPath)
{
	m_pDllPath = pcDllPath;

	// Load dll of machine vision librry
	m_hDllLibrary = LoadLibrary(TEXT(m_pDllPath));

	if(m_hDllLibrary == NULL)
	{
		// Exit failure
		return false;
	}
	else
	{
		true;
	}
}

bool MVL_API::SetProperty(int nPropertyID, void* pValue, void* pValue2, void* pValue3)
{
	switch(nPropertyID)
	{
		case MVL_API_CREATE:
			m_Create = (MVL_Create_Func) GetProcAddress(m_hDllLibrary, m_pMVL_Create);
			break;

		case MVL_API_DESTROY:
			m_Destroy = (MVL_Destroy_Func) GetProcAddress(m_hDllLibrary, m_pMVL_Destroy);
			break;

		case MVL_API_SET_PROPERTY:
			m_SetProperty = (MVL_SetProperty_Func) GetProcAddress(m_hDllLibrary, m_pMVL_SetProperty);
			break;

		case MVL_API_GET_PROPERTY:
			m_GetProperty = (MVL_GetProperty_Func) GetProcAddress(m_hDllLibrary, m_pMVL_GetProperty);
			break;

		case MVL_API_ANALYZE:
			m_Analyze = (MVL_Analyze_Func) GetProcAddress(m_hDllLibrary, m_pMVL_Analyze);
			break;

		case MVL_API_GET_LAST_ERROR:
			m_GetLastError = (MVL_GetLastError_Func) GetProcAddress(m_hDllLibrary, m_pMVL_GetLastError);
			break;

		default:
			return false;
	}

	return true;
}

bool MVL_API::GetProperty(int nPropertyID, void* pValue, void* pValue2, void* pValue3)
{
	switch(nPropertyID)
	{
		case MVL_API_CREATE:
			if(((MVL_Create_Func*)pValue) != NULL)
			{
				*(MVL_Create_Func*)pValue = m_Create;
			}
			break;

		case MVL_API_CREATE_READ:
			strcpy((char*)pValue, m_pMVL_Create);
			break;

		case MVL_API_DESTROY:
			if(((MVL_Destroy_Func*)pValue) != NULL)
			{
				*(MVL_Destroy_Func*)pValue = m_Destroy;
			}
			break;

		case MVL_API_DESTROY_READ:
			strcpy((char*)pValue, m_pMVL_Destroy);
			break;

		default:
			return false;
	}

	return true;
}

bool MVL_API::CallMethod(int nPropertyID, void* pValue, void* pValue2, void* pValue3)
{
	switch(nPropertyID)
	{
		case MVL_API_CREATE:
			// Create of Library
			if(m_Create != NULL)
			{
				(m_Create)((char*)pValue);
			}
			break;

		case MVL_API_SET_PROPERTY:
			(m_SetProperty)(nPropertyID, pValue, pValue2, pValue3);
			break;

		case MVL_API_GET_PROPERTY:
			(m_GetProperty)(nPropertyID, pValue, pValue2, pValue3);
			break;

		case MVL_API_ANALYZE:
			(m_Analyze)();
			break;

		case MVL_API_GET_LAST_ERROR:
			m_GetLastError = (MVL_GetLastError_Func) GetProcAddress(m_hDllLibrary, m_pMVL_GetLastError);
			break;

		case MVL_API_DESTROY:
			// Destroy of Library
			if(m_Destroy != NULL)
			{
				(m_Destroy)();
				FreeLibrary(m_hDllLibrary);
			}
			break;

		default:
			return false;
	}

	return true;
}