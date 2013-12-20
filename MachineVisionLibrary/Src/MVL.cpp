#include "OpenCVHeader.h"
#include "stdio.h"
#include <MVL.h>
#include "Workspace.h"

using namespace MVL;

static CWorkspace Workspace;

MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_Create(char* pImagePath)
{
	if(Workspace.Create(pImagePath) == true)
	{
		return MVL_RETURN_SUCCESS;
	}
	else
	{
		return MVL_RETURN_FAILURE;
	}
}

MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_Destroy()
{
	Workspace.~CWorkspace();

	return MVL_RETURN_SUCCESS;
}

MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_SetProperty(int nParameterId, void* pValue, void* pValue2, void* pValue3)
{
	if(Workspace.SetParameter(nParameterId, pValue, pValue2, pValue3) == true)
	{
		return MVL_RETURN_SUCCESS;
	}
	else
	{
		return MVL_RETURN_FAILURE;
	}
}

MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_GetProperty(int nParameterId, void* pValue, void* pValue2, void* pValue3)
{
	if(Workspace.GetParameter(nParameterId, pValue, pValue2, pValue3) == true)
	{
		return MVL_RETURN_SUCCESS;
	}
	else
	{
		return MVL_RETURN_FAILURE;
	}
}

MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_Analyze()
{
	if(Workspace.Analyze() == true)
	{
		return MVL_RETURN_SUCCESS;
	}
	else
	{
		return MVL_RETURN_FAILURE;
	}
}

MVL_INTERFACE_EXPORT MVL_ERROR_CODE MVL_CALLING MVL_GetLastError()
{
	return Workspace.GetLastError();
}
