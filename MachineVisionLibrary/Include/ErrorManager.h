#ifndef _ERROR_MANAGER_HEADER
#define _ERROR_MANAGER_HEADER

#include "MVL.h"
#include <vector>

namespace MVL
{
	class CErrorManager
	{
		public:
			CErrorManager();
			bool Reset();

			bool SendError(MVL_ERROR_CODE eError);
			MVL_ERROR_CODE GetLastError();

		private:
			std::vector<MVL_ERROR_CODE> m_vErrorQueue;
	};
};

#endif // _ERROR_MANAGER_HEADER