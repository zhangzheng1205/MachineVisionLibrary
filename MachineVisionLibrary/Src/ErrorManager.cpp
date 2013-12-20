#include "ErrorManager.h"

namespace MVL
{
	CErrorManager::CErrorManager()
	{
	}

	bool CErrorManager::Reset()
	{
		m_vErrorQueue.clear();
		return true;
	}

	bool CErrorManager::SendError(MVL_ERROR_CODE eError)
	{
		m_vErrorQueue.push_back(eError);
		return true;
	}

	MVL_ERROR_CODE CErrorManager::GetLastError()
	{
		MVL_ERROR_CODE eError = MVL_ERROR_NOT_PRESENT;
		if(m_vErrorQueue.back() - 1 != NULL)
		{
			eError = *m_vErrorQueue.end();
			m_vErrorQueue.pop_back();
		}
		else
		{
			return eError;
		}
	}
};