#include <Result.h>
#include <stdio.h>

namespace MVL
{
	CAnalizerEdgeResult::CAnalizerEdgeResult(MVL_ANALYSIS eAnalysis)
	{
		m_nNumPoints = 0;
		m_nMaxPoints = 0;
		pnXCoordinates = pnYCoordinates = NULL;
		pucPointsValues = NULL;

		m_eCurrentAnalizer = eAnalysis;
	}

	CAnalizerEdgeResult::~CAnalizerEdgeResult()
	{
		DestroyArray();
	}

	void CAnalizerEdgeResult::CreateArray()
	{
		if(pnXCoordinates == NULL)
		{
			pnXCoordinates = new int[m_nMaxPoints];
		}
		if(pnYCoordinates == NULL)
		{
			pnYCoordinates = new int[m_nMaxPoints];
		}
		if(pucPointsValues == NULL)
		{
			pucPointsValues = new unsigned char[m_nMaxPoints];
		}
		m_nNumPoints = 0;
	}

	void CAnalizerEdgeResult::DestroyArray()
	{
		if(pnXCoordinates != NULL)
		{
			delete[] pnXCoordinates;
			pnXCoordinates = NULL;
		}
		if(pnYCoordinates != NULL)
		{
			delete[] pnYCoordinates;
			pnYCoordinates = NULL;
		}
		if(pucPointsValues != NULL)
		{
			delete[] pucPointsValues;
			pucPointsValues = NULL;
		}
		m_nNumPoints = 0;
	}

	bool CAnalizerEdgeResult::SetMaxPointNumber(int nMaxPoints)
	{
		if((m_nMaxPoints > 0) || (nMaxPoints <= 0))
		{
			return false;
		}

		DestroyArray();
		m_nMaxPoints = nMaxPoints;
		CreateArray();
		return true;
	}

	bool CAnalizerEdgeResult::AddPoint(int nXCoord, int nYCoord, unsigned char ucValue)
	{
		if(m_nNumPoints < m_nMaxPoints)
		{
			pnXCoordinates[m_nNumPoints] = nXCoord;
			pnYCoordinates[m_nNumPoints] = nYCoord;
			pucPointsValues[m_nNumPoints] = ucValue;
			++m_nNumPoints;
			return true;
		}
		else
		{
			return false;
		}
	}

	bool CAnalizerEdgeResult::CopyContent(int* pnOutVals)
	{
		int nCount = 0;
		for(int i=0; i<m_nNumPoints; i++)
		{
			pnOutVals[nCount++] = pnXCoordinates[i];
			pnOutVals[nCount++] = pnYCoordinates[i];
			//pnOutVals[nCount++] = pucPointsValues[i];
		}
		return true;
	}

	CResult::CResult(CWorkspace* pWorkspace)
	{
		m_pWorkspace = pWorkspace;
		m_pvAnalizersResultsEdges.clear();
		m_eCurrentAnalizer = MVL_UNDEFINED_ANALISYS;
	}

	CResult::~CResult()
	{
		DeletePreviousResults();
	}

	bool CResult::Initialize()
	{
		return true;
	}

	bool CResult::Reset()
	{
		DeletePreviousResults();
		return true;
	}

	bool CResult::SetProperty(ERESULT_PROPERTY ePropertyID, void* pPropertyValue)
	{
		if(pPropertyValue == NULL)
		{
			return false;
		}

		switch(ePropertyID)
		{
			case ERESULT_SAVE_ANALIZER_EDGES:
					m_pvAnalizersResultsEdges.push_back((CAnalizerEdgeResult*)pPropertyValue);
				break;

			case ERESULT_SELECT_ANALIZER_ID:
				{
					m_eCurrentAnalizer = *(MVL_ANALYSIS*)pPropertyValue;

					// Look for a result of that analizer
					CAnalizerEdgeResult* pAnalizerResult = GetAnalizerResult();
					if(pAnalizerResult == NULL)
					{
						return false;
					}
				}
				break;

			default:
				return false;
		}

		return true;
	}

	bool CResult::GetProperty(ERESULT_PROPERTY ePropertyID, void* pPropertyValue)
	{
		if(pPropertyValue == NULL)
		{
			return false;
		}

		switch(ePropertyID)
		{
			case ERESULT_GET_ANALIZER_OUTPUT:
				{
					CAnalizerEdgeResult* pAnalizerResult = GetAnalizerResult();
					if(pAnalizerResult == NULL)
					{
						return false;
					}
					pAnalizerResult->CopyContent((int*)pPropertyValue);
				}
				break;

			case ERESULT_GET_ANALIZER_OUTPUT_NUM_POINTS:
				{
					CAnalizerEdgeResult* pAnalizerResult = GetAnalizerResult();
					if(pAnalizerResult == NULL)
					{
						return false;
					}
					*(int*)pPropertyValue = pAnalizerResult->GetNumPoints();
				}
				break;

			default:
				return false;
		}

		return true;
	}

	void CResult::DeletePreviousResults()
	{
		if(m_pvAnalizersResultsEdges.size() == 0)
		{
			return;
		}

		for(std::vector<CAnalizerEdgeResult*>::iterator i=m_pvAnalizersResultsEdges.begin(); i<m_pvAnalizersResultsEdges.end(); ++i)
		{
			if(*i != NULL)
			{
				(*i)->~CAnalizerEdgeResult();
			}
		}

		m_pvAnalizersResultsEdges.clear();
	}

	CAnalizerEdgeResult* CResult::GetAnalizerResult()
	{
		for(std::vector<CAnalizerEdgeResult*>::iterator i=m_pvAnalizersResultsEdges.begin(); i<m_pvAnalizersResultsEdges.end(); ++i)
		{
			if((*i != NULL) && ((*i)->GetAnalizer() == m_eCurrentAnalizer))
			{
				return *i;
			}
		}

		return NULL;
	}
}