#ifndef _RESULT_HEADER_
#define _RESULT_HEADER_

#include <MVL.h>
#include <vector>

namespace MVL
{
	typedef enum _ERESULT_PROPERTY
	{
		ERESULT_UNDEFINED_PROPERTY	= -1,

		ERESULT_SAVE_ANALIZER_EDGES	= 0,
		ERESULT_SELECT_ANALIZER_ID,
		ERESULT_GET_ANALIZER_OUTPUT,
		ERESULT_GET_ANALIZER_OUTPUT_NUM_POINTS,

		// Tha last one
		ERESULT_NUMBER_OF_PROPERTIES

	} ERESULT_PROPERTY;

	class CAnalizerEdgeResult
	{
	public:
		CAnalizerEdgeResult(MVL_ANALYSIS eAnalysis);
		~CAnalizerEdgeResult();

		bool SetMaxPointNumber(int nMaxPoints);
		bool AddPoint(int nXCoord, int nYCoord, unsigned char ucValue);
		MVL_ANALYSIS GetAnalizer(){return m_eCurrentAnalizer;};
		int GetNumPoints(){return m_nNumPoints;};
		bool CopyContent(int* pnOutVals);

	private:
		void CreateArray();
		void DestroyArray();

		MVL_ANALYSIS m_eCurrentAnalizer;
		int m_nMaxPoints;
		int m_nNumPoints;
		int* pnXCoordinates;
		int* pnYCoordinates;
		unsigned char* pucPointsValues;
	};

	class CWorkspace;

	class CResult
	{
	public:
		CResult(CWorkspace* pWorkspace);
		~CResult();

		bool Initialize();
		bool Reset();
		bool SetProperty(ERESULT_PROPERTY ePropertyID, void* pPropertyValue);
		bool GetProperty(ERESULT_PROPERTY ePropertyID, void* pPropertyValue);

	private:
		void DeletePreviousResults();
		CAnalizerEdgeResult* GetAnalizerResult();

		CWorkspace* m_pWorkspace;
		std::vector<CAnalizerEdgeResult*> m_pvAnalizersResultsEdges;
		MVL_ANALYSIS m_eCurrentAnalizer;
	};
};


#endif //_RESULT_HEADER_