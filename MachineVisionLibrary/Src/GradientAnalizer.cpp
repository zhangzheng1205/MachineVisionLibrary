#include "Image.h"
#include "GradientAnalizer.h"
#include "MVL.h"
#include "PreProcessor.h"
#include "string.h"
#include "math.h"
#include <Workspace.h>
#include <Result.h>

namespace MVL
{
	CGradientAnalizer::CGradientAnalizer()
	: CAnalizer(MVL_GRADIENTE_ANALYSIS)
	{
	}

	CGradientAnalizer::~CGradientAnalizer()
	{
	}

	bool CGradientAnalizer::Initialize()
	{
		return true;
	}

	bool CGradientAnalizer::ReInitialize(int nReinitType)
	{
		return true;
	}

	bool CGradientAnalizer::SetProperty(int nPropertyId, void* pPropertyValue)
	{
		switch(nPropertyId)
		{
			default:
				return false;
		}

		return true;
	}

	bool CGradientAnalizer::GetProperty(int nPropertyId, void* pPropertyValue)
	{
		switch(nPropertyId)
		{
			default:
				return false;
		}

		return true;
	}

	bool CGradientAnalizer::AnalyzeImage(CWorkspace* pWorkspace)
	{
		int nWidth;
		int nWidthStep;
		int nHeight;
		unsigned char* pImageData = NULL;
		int nValue = 0;
		int nNewWidthStep = 0;
		CAnalizerEdgeResult* pResultData = NULL;
		CResult* pResult = (CResult*)pWorkspace->GetWorkspaceObject(EMVL_RESULT_OBJECT_ID);

		// Get image parameters
		m_pImage->GetProperty(EIMAGE_WIDTH, &nWidth);
		m_pImage->GetProperty(EIMAGE_WIDTHSTEP, &nWidthStep);
		m_pImage->GetProperty(EIMAGE_HEIGHT, &nHeight);
		m_pImage->GetProperty(EIMAGE_DATA_POINTER, &pImageData);

		m_pImage->ReloadDebugImage();

		int nXCoord=0;
		int nYCoord=0;
		bool bSign = false;
		int nNumPoints = 0;

		// Sobel operator
		for(int j=1; j<nHeight-1; j++)
		{
			for(int i=1; i<nWidth-1; i++)
			{
				nValue = (int)(
						  abs(0.25 * (-pImageData[(j-1)*nWidthStep + (i-1)] -2 * pImageData[(j)*nWidthStep + (i-1)] -
								  pImageData[(j+1)*nWidthStep + (i-1)] + pImageData[(j-1)*nWidthStep + (i+1)] +
								  2 * pImageData[(j)*nWidthStep + (i+1)] + pImageData[(j+1)*nWidthStep + (i+1)])) +
						  abs(0.25 * (-pImageData[(j-1)*nWidthStep + (i-1)] -2 * pImageData[(j-1)*nWidthStep + (i)] -
								   pImageData[(j-1)*nWidthStep + (i+1)] + pImageData[(j+1)*nWidthStep + (i-1)] +
								   2 * pImageData[(j+1)*nWidthStep + (i)] + pImageData[(j+1)*nWidthStep + (i+1)]))
										  );
				if((unsigned char)nValue > 10)
				{
					++nNumPoints;
				}
				m_pImage->DrawPointDebug(i, j, (unsigned char)nValue);
			}
		}		

		pResultData = new CAnalizerEdgeResult(MVL_GRADIENTE_ANALYSIS);
		pResultData->SetMaxPointNumber(nNumPoints);

		for(int j=1; j<nHeight-1; j++)
		{
			for(int i=1; i<nWidth-1; i++)
			{
				nValue = (int)(
						  abs(0.25 * (-pImageData[(j-1)*nWidthStep + (i-1)] -2 * pImageData[(j)*nWidthStep + (i-1)] -
								  pImageData[(j+1)*nWidthStep + (i-1)] + pImageData[(j-1)*nWidthStep + (i+1)] +
								  2 * pImageData[(j)*nWidthStep + (i+1)] + pImageData[(j+1)*nWidthStep + (i+1)])) +
						  abs(0.25 * (-pImageData[(j-1)*nWidthStep + (i-1)] -2 * pImageData[(j-1)*nWidthStep + (i)] -
								   pImageData[(j-1)*nWidthStep + (i+1)] + pImageData[(j+1)*nWidthStep + (i-1)] +
								   2 * pImageData[(j+1)*nWidthStep + (i)] + pImageData[(j+1)*nWidthStep + (i+1)]))
										  );
				if((unsigned char)nValue > 10)
				{
					pResultData->AddPoint(i, j, (unsigned char)nValue);
				}
			}
		}

		pResult->SetProperty(ERESULT_SAVE_ANALIZER_EDGES, pResultData);

#ifdef MVL_GRAPHIC_ENABLE
		m_pImage->ShowImageDebug(10, true);
		m_pImage->ShowImage(10, true);
#endif
		return true;
	}
}