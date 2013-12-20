#ifndef _MVL_SETTINGS_HEADER
#define _MVL_SETTINGS_HEADER

#include <MVL.h>

namespace MVL
{
	class CSettings
	{
		public:
			CSettings();
			~CSettings();

			bool Reset();

			bool m_pbAnalysis[MVL_NUMBER_OF_ANALISYS];
	};
}

#endif //_MVL_SETTINGS_HEADER