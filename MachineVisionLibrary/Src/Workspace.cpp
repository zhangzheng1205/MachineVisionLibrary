#include "Workspace.h"
#include "stdlib.h"
#include "MVL_Version.h"
#include "GradientAnalizer.h"

namespace MVL
{
	CWorkspace* CWorkspace::s_pWorkspaces[MVLD_MAX_NUM_WORKSPACES];
	int CWorkspace::s_nNumworkspacesCreated = 0;

	void* CWorkspace::GetMVLObject(EMVL_OBJECT_ID eObject, int nWorkspaceID)
	{
		if(nWorkspaceID < 0 || nWorkspaceID >= s_nNumworkspacesCreated)
		{
			return NULL;
		}
		
		return s_pWorkspaces[nWorkspaceID]->GetWorkspaceObject(eObject);
	}

	CWorkspace::CWorkspace()
	{
		// Save itself
		s_pWorkspaces[s_nNumworkspacesCreated] = this;
		m_nWorkspaceID = s_nNumworkspacesCreated;
		s_nNumworkspacesCreated++;

		m_pImage = NULL;
		m_pSettings = NULL;
		m_pErrorManager = NULL;
		m_pcImagePath = NULL;

		// Reset analizers
		for(int i=0; i<MVL_NUMBER_OF_ANALISYS; i++)
		{
			m_pAnalizers[i] = NULL;
		}

		m_pResult = NULL;
	}

	CWorkspace::~CWorkspace()
	{
		if(m_pImage != NULL)
		{
			delete m_pImage;
			m_pImage = NULL;
		}

		if(m_pSettings != NULL)
		{
			delete m_pSettings;
			m_pSettings = NULL;
		}

		if(m_pErrorManager != NULL)
		{
			delete m_pErrorManager;
			m_pErrorManager = NULL;
		}

		// Destroy analizers
		for(int i=0; i<MVL_NUMBER_OF_ANALISYS; i++)
		{
			if( m_pAnalizers[i] != NULL)
			{
				delete m_pAnalizers[i];
				m_pAnalizers[i] = NULL;
			}
		}

		if(m_pResult != NULL)
		{
			delete m_pResult;
			m_pResult = NULL;
		}

		// Remove itself
		s_pWorkspaces[m_nWorkspaceID] = NULL;
		if(m_nWorkspaceID == s_nNumworkspacesCreated - 1)
		{
			--s_nNumworkspacesCreated;
		}
	}

	void* CWorkspace::GetWorkspaceObject(EMVL_OBJECT_ID eObject)
	{
		switch(eObject)
		{
			case EMVL_WORKSPACE_OBJECT_ID:
				return this;

			case EMVL_IMAGE_OBJECT_ID:
				return m_pImage;

			case EMVL_GRADIENT_ANALIZER_OBJECT_ID:
				return m_pAnalizers[MVL_GRADIENTE_ANALYSIS];

			case EMVL_SETTINGS_OBJECT_ID:
				return m_pSettings;

			case EMVL_ERROR_MANAGER_OBJECT_ID:
				return m_pErrorManager;

			case EMVL_RESULT_OBJECT_ID:
				return m_pResult;

			default:
				return NULL;
		}
	}

	bool CWorkspace::Create(char* pcImage, void* pParameters)
	{
		// Create workspace objects
		if(m_pImage == NULL)
		{
			m_pImage = new CImage();
			
			if(pcImage != NULL)
			{
				// Load Image
				m_pImage->LoadImage(pcImage);
				m_pcImagePath = pcImage;
			}
			else
			{
				m_pcImagePath = NULL;
			}
		}

		if(m_pSettings == NULL)
		{
			m_pSettings = new CSettings();
		}

		if(m_pErrorManager == NULL)
		{
			m_pErrorManager = new CErrorManager();
		}

		// Create analizers and initialize
		for(int i=0; i<MVL_NUMBER_OF_ANALISYS; i++)
		{
			// Create
			switch(i)
			{
				case MVL_GRADIENTE_ANALYSIS:
					m_pAnalizers[MVL_GRADIENTE_ANALYSIS] = new CGradientAnalizer();
					break;

				default:
					m_pAnalizers[i] = NULL;
					continue;
			}

			// Initialize
			m_pAnalizers[i]->Initialize();
		}

		if(m_pResult == NULL)
		{
			m_pResult = new CResult(this);

			m_pResult->Initialize();
		}

		return true;
	}

	bool CWorkspace::SetParameter(int nParameterId, void* pValue, void* pValue2, void* pValue3)
	{
		switch(nParameterId)
		{
			case MVL_SELECT_ANALYSIS:
				{
					MVL_ANALYSIS eAnalysis = *(MVL_ANALYSIS*)pValue;
					MVL_VALUE_TYPE eValue = *(MVL_VALUE_TYPE*)pValue2;
					if((eAnalysis > MVL_UNDEFINED_ANALISYS) && (eAnalysis < MVL_NUMBER_OF_ANALISYS))
					{
						m_pSettings->m_pbAnalysis[eAnalysis] = (eValue == MVL_VALUE_ENABLED)? true : false;
					}
					else
					{
						m_pErrorManager->SendError(MVL_ERROR_PROPERTY_WRONG_VALUE);
					}
				}
				break;

			// Used to change image
			case MVL_PROPERTY_IMAGE_ADDRESS:
				{
					char* pcImageAddress = (char*)pValue;
					
					if(pcImageAddress == NULL)
					{
						return false;
					}

					delete m_pImage;
					m_pImage = new CImage();

					if(m_pImage->LoadImage(pcImageAddress) == false)
					{
						return false;
					}

					m_pcImagePath = pcImageAddress;
				}
				break;

			case MVL_PROPERTY_RESULT_ANALIZER_ID:
				{
					int* pnVal = NULL;
					MVL_ANALYSIS eAnalysis;

					pnVal = (int*)pValue;
					if(pnVal == NULL)
					{
						return false;
					}
					eAnalysis = *(MVL_ANALYSIS*)pnVal;


					m_pResult->SetProperty(ERESULT_SELECT_ANALIZER_ID, &eAnalysis);
				}
				break;

			default:
				return false;
		}

		return true;
	}

	bool CWorkspace::GetParameter(int nParameterId, void* pValue, void* pValue2, void* pValue3)
	{
		char* pcData = NULL;
		int* pnVal = NULL;

		switch(nParameterId)
		{
			case MVL_VERSION_ID:
				
				pcData = (char*)pValue;
				strcpy(pcData, MVL_VERSION_IDENTIFICATION);
				break;

			case MVL_PROPERTY_RESULT_ANALIZER_NUM_POINTS:
				{
					int nVal = 0;
					pnVal = (int*)pValue;

					// Here obtain number of points
					m_pResult->GetProperty(ERESULT_GET_ANALIZER_OUTPUT_NUM_POINTS, &nVal);

					*pnVal = nVal;
				}
				break;

			case MVL_PROPERTY_RESULT_ANALIZER_NUM_BYTES:
				{
					int nVal = 0;
					pnVal = (int*)pValue;

					// Here obtain number of points
					m_pResult->GetProperty(ERESULT_GET_ANALIZER_OUTPUT_NUM_POINTS, &nVal);

					// In order to get number of bytes, each point is composed by two coordinates, 
					// one value and first two are integers, the last one is an unsigned character
					*pnVal = nVal * (2*sizeof(int));
				}
				break;

			case MVL_PROPERTY_RESULT_ANALIZER_DATA:
				{
					m_pResult->GetProperty(ERESULT_GET_ANALIZER_OUTPUT, (int*)pValue);
				}
				break;

			case MVL_PROPERTY_GET_IMAGE_WIDTH:
				m_pImage->GetProperty(EIMAGE_WIDTH, pValue);
				break;

			case MVL_PROPERTY_GET_IMAGE_HEIGHT:
				m_pImage->GetProperty(EIMAGE_HEIGHT, pValue);
				break;

			default:
				return false;
		}

		return true;
	}

	bool CWorkspace::Analyze()
	{
		// Security check
		if(m_pcImagePath == NULL)
		{
			return false;
		}

		// Reset error manager
		m_pErrorManager->Reset();

		// Reset result object
		m_pResult->Reset();

		// Re-Initialize analizers
		for(int i=0; i<MVL_NUMBER_OF_ANALISYS; i++)
		{
			if(m_pAnalizers[i] != NULL)
			{
				m_pAnalizers[i]->ReInitialize(EANALIZER_REINIT_IMAGE);
			}
		}

		// Cycle on all types of analysis setted
		for(int i=0; i<MVL_NUMBER_OF_ANALISYS; ++i)
		{
			if((m_pSettings->m_pbAnalysis[i] == true) &&
				(m_pAnalizers[i] != NULL))
			{
				bool bValue = false;

				// Execute analysis
				bValue = m_pAnalizers[i]->Analyze(m_pImage, this);
				
				if(bValue == false)
				{
					m_pErrorManager->SendError(MVL_ERROR_ANALIZER_WORK);
				}
			}
		}
		
		return true;
	}
}