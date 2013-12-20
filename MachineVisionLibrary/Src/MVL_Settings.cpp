#include <MVL_Settings.h>
#include <string.h>

namespace MVL
{
	CSettings::CSettings()
	{
		Reset();
	}
	
	CSettings::~CSettings()
	{
	}

	bool CSettings::Reset()
	{
		for(int i=0; i<MVL_NUMBER_OF_ANALISYS; ++i)
		{
			m_pbAnalysis[i] = false;
		}
		return true;
	}
}
