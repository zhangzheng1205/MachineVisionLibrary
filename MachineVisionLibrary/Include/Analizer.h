#ifndef _ANALIZER_HEADER
#define _ANALIZER_HEADER

#include <stdio.h>
#include "MVL.h"

namespace MVL
{
	class CImage;
	class CWorkspace;

	// Defines types of re init
	typedef enum _EANALIZER_REINIT_TYPE
	{
		EANALIZER_REINIT_IMAGE	= 0,

	} EANALIZER_REINIT_TYPE;

	class CAnalizer
	{
	public:
		CAnalizer(MVL_ANALYSIS eType)
			: m_eType(eType)
		{
			m_pImage = NULL;
		};
		virtual ~CAnalizer()
		{
			m_pImage = NULL;
			m_eType = MVL_UNDEFINED_ANALISYS;
		};

		virtual bool Initialize() = 0;
		virtual bool ReInitialize(int nReinitType)	= 0;

		virtual bool SetProperty(int nPropertyId, void* pPropertyValue)	 = 0;
		virtual bool GetProperty(int nPropertyId, void* pPropertyValue)	 = 0;

		bool Analyze(CImage* pImage, CWorkspace* pWorkspace)
		{
			m_pImage = pImage;
			return AnalyzeImage(pWorkspace);
		};

		MVL_ANALYSIS GetType()
		{
			return m_eType;
		};

	protected:
		virtual bool AnalyzeImage(CWorkspace* pWorkspace) = 0;

		CImage* m_pImage;

	private:
		MVL_ANALYSIS m_eType;
	};
};

#endif //_ANALIZER_HEADER