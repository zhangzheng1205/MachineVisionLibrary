#ifndef _GRADIENT_ANALIZER_HEADER
#define _GRADIENT_ANALIZER_HEADER

#include "Analizer.h"

namespace MVL
{
	class CGradientAnalizer : public CAnalizer
	{
	public:
		CGradientAnalizer();
		virtual ~CGradientAnalizer();
		virtual bool Initialize();
		virtual bool ReInitialize(int nReinitType);

		virtual bool SetProperty(int nPropertyId, void* pPropertyValue);
		virtual bool GetProperty(int nPropertyId, void* pPropertyValue);

	protected:
		virtual bool AnalyzeImage(CWorkspace* pWorkspace);
	};
};

#endif // _GRADIENT_ANALIZER_HEADER