#ifndef _WORKSPACE_HEADER
#define _WORKSPACE_HEADER

#include "Image.h"
#include "stdio.h"
#include "PreProcessor.h"
#include "MVL.h"
#include "MVL_Settings.h"
#include "ErrorManager.h"
#include "Analizer.h"
#include <Result.h>
#include <Object_Identification.h>

namespace MVL
{
#define MVLD_MAX_NUM_WORKSPACES	(10)

	class CWorkspace
	{
	public:
		CWorkspace();
		~CWorkspace();

		bool Create(char* pcImage, void* pParameters = NULL);
		bool SetParameter(int nParameterId, void* pValue, void* pValue2, void* pValue3);
		bool GetParameter(int nParameterId, void* pValue, void* pValue2, void* pValue3);
		bool Analyze();

		void* GetWorkspaceObject(EMVL_OBJECT_ID eObject);
		static void* GetMVLObject(EMVL_OBJECT_ID eObject, int nWorkspaceID = 0);

		MVL_ERROR_CODE GetLastError(){ return m_pErrorManager->GetLastError();};

	private:
		// Save all workspaces that are created
		static CWorkspace* s_pWorkspaces[MVLD_MAX_NUM_WORKSPACES];
		static int s_nNumworkspacesCreated;

		int m_nWorkspaceID;
		CImage* m_pImage;
		CSettings* m_pSettings;
		CErrorManager* m_pErrorManager;
		CAnalizer* m_pAnalizers[MVL_NUMBER_OF_ANALISYS];
		CResult* m_pResult;

		char* m_pcImagePath;
	};

};
#endif //_WORKSPACE_HEADER