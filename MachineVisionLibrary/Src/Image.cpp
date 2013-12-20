#include <Image.h>
#include <PreProcessor.h>
#include <CommonMacro.h>

namespace MVL
{
#ifdef MVL_GRAPHIC_ENABLE
	#define _RED_COLOR		(cvScalar(0, 0, 255))
	#define _GREEN_COLOR	(cvScalar(0, 255, 0))
	#define _BLUE_COLOR		(cvScalar(255, 0, 0))
	#define _BLACK_COLOR	(cvScalar(255, 255, 255))
	#define _WHITE_COLOR	(cvScalar(0, 0, 0))
#endif

	int CImage::s_nNumImageIndex = 0;
	std::vector<int> CImage::s_vIndexImagesHistory;

	CImage::CImage()
	{
		m_pCurrentImage =  m_pWorkImage = m_pWorkImageDebug = NULL;
		m_nNumImageShowed = 0;
		
		// Set index of image created
		m_nIndexImage = s_nNumImageIndex;
		s_vIndexImagesHistory.push_back(m_nIndexImage);
		// Increment number of images created
		++s_nNumImageIndex;

		m_nIndexImageColor = 0;
		m_nIndexImageDebug = 0;

		m_NumImagesTemporaryCreated = 0;
		m_vTempImagesVector.clear();
	}

	CImage::~CImage()
	{
		if(m_pCurrentImage != NULL)
		{
			cvReleaseImage(&m_pCurrentImage);
			m_pCurrentImage = NULL;
		}

		if(m_pWorkImage != NULL)
		{
			cvReleaseImage(&m_pWorkImage);
			m_pWorkImage = NULL;
		}

		if(m_pWorkImageDebug != NULL)
		{
			cvReleaseImage(&m_pWorkImageDebug);
			m_pWorkImageDebug = NULL;
		}

		DestroyAllWindowsShowed();

		// Remove index of image from list of indexes associated to image objects
		if(m_nIndexImage == s_nNumImageIndex - 1)
		{
			s_vIndexImagesHistory.pop_back();
			--s_nNumImageIndex;
		}
		else
		{
			for(std::vector<int>::iterator i=s_vIndexImagesHistory.begin(); i<s_vIndexImagesHistory.end(); ++i)
			{
				if(*i == m_nIndexImage)
				{
					s_vIndexImagesHistory.erase(i);
					m_nIndexImage = -1;
					break;
				}
			}
		}

		for(std::vector<IplImage*>::iterator i=m_vTempImagesVector.begin(); i<m_vTempImagesVector.end(); ++i)
		{
			cvReleaseImage(&(*i));
			m_vTempImagesVector.erase(i);
		}
		m_NumImagesTemporaryCreated = 0;
	}

	bool CImage::LoadImage(const char* pImagePath)
	{
		bool bValue = false;

		if(m_pCurrentImage != NULL)
		{
			cvReleaseImage(&m_pCurrentImage);
			m_pCurrentImage = NULL;
		}

		// Load image from file system
		IplImage* pTempImage = cvLoadImage(pImagePath);

		// Create image used to display debug drawings
		if(pTempImage->nChannels == 1)
		{
			m_pCurrentImage = cvCreateImage(cvSize(pTempImage->width, pTempImage->height), pTempImage->depth, 3);
			cvZero(m_pCurrentImage);
			cvCvtColor(pTempImage, m_pCurrentImage, CV_GRAY2BGR);
			cvReleaseImage(&pTempImage);
		}
		else
		{
			m_pCurrentImage = pTempImage;
		}

		CreateWorkImage();

		if(m_pCurrentImage != NULL)
		{
			bValue = true;
			m_nImageWidth = m_pWorkImage->width;
			m_nImageWidthStep = m_pWorkImage->widthStep;
			m_nImageHeight = m_pWorkImage->height;
		}

		return bValue; 
	}

	bool CImage::LoadImage(IplImage* pImage)
	{
		bool bValue = false;

		if(pImage == NULL)
		{
			return false;
		}

		if(m_pCurrentImage != NULL)
		{
			cvReleaseImage(&m_pCurrentImage);
			m_pCurrentImage = NULL;
		}

		IplImage* pTempImage = cvCreateImage(cvGetSize(pImage), pImage->depth, pImage->nChannels);
		cvCopy(pImage, pTempImage);

		// Create image used to display debug drawings
		if(pTempImage->nChannels == 1)
		{
			m_pCurrentImage = cvCreateImage(cvGetSize(pTempImage), pTempImage->depth, 3);
			cvZero(m_pCurrentImage);
			cvCvtColor(pTempImage, m_pCurrentImage, CV_GRAY2BGR);
			cvReleaseImage(&pTempImage);
		}
		else
		{
			m_pCurrentImage = pTempImage;
		}

		CreateWorkImage();

		if(m_pCurrentImage != NULL)
		{
			bValue = true;
			m_nImageWidth = m_pWorkImage->width;
			m_nImageWidthStep = m_pWorkImage->widthStep;
			m_nImageHeight = m_pWorkImage->height;
		}

		return bValue;
	}

	unsigned char* CImage::CreateTempImageUC(int nWidth, int nHeight, int* pnWidthStep)
	{
		IplImage* ptr = cvCreateImage(cvSize(nWidth, nHeight), CV_8U, m_pWorkImage->nChannels);
		
		if(ptr == NULL)
		{
			*pnWidthStep = -1;
			return NULL;
		}
		else
		{
			cvZero(ptr);
			*pnWidthStep = ptr->widthStep;

			++m_NumImagesTemporaryCreated;
			m_vTempImagesVector.push_back(ptr);

			return (unsigned char*)ptr->imageData;
		}
	}

	int* CImage::CreateTempImage32S(int nWidth, int nHeight, int* pnWidthStep)
	{
		IplImage* ptr = cvCreateImage(cvSize(nWidth, nHeight), 32, m_pWorkImage->nChannels);
		
		if(ptr == NULL)
		{
			*pnWidthStep = -1;
			return NULL;
		}
		else
		{
			cvZero(ptr);
			*pnWidthStep = ptr->widthStep;

			++m_NumImagesTemporaryCreated;
			m_vTempImagesVector.push_back(ptr);

			return (int*)ptr->imageData;
		}
	}

	int CImage::ShowImage(int nWaitTime, bool bNewImage)
	{
		char pcImageName[30] = "";
		int nIndexImage = -1;

		if((bNewImage == true) || (m_nIndexImageColor == 0))
		{
			sprintf(pcImageName, "Image_%d_%d", m_nIndexImage, m_nIndexImageColor);

			m_vWindowIndex.push_back(m_nIndexImageColor);
			nIndexImage = m_nIndexImageColor;
			++m_nIndexImageColor;

			// Increment number of images showed
			++m_nNumImageShowed;
		}
		else
		{
			sprintf(pcImageName, "Image_%d_%d", m_nIndexImage, m_nIndexImageColor-1);
			nIndexImage = m_nIndexImageColor - 1;
		}

		cvNamedWindow(pcImageName, 0);
		cvResizeWindow(pcImageName, m_pCurrentImage->width, m_pCurrentImage->height);
		cvShowImage(pcImageName, m_pCurrentImage);

		cvWaitKey(nWaitTime);

		return nIndexImage;
	}

	int CImage::ShowImageDebug(int nWaitTime, bool bNewImage)
	{
		int nIndexImage = -1;
		char pcImageName[40] = "";

		if((bNewImage == true) || (m_nIndexImageDebug == 0))
		{
			sprintf(pcImageName, "Image_%d_DEBUG_%d", m_nIndexImage, m_nIndexImageDebug);

			m_vWindowIndexDebug.push_back(m_nIndexImageDebug);
			nIndexImage = m_nIndexImageDebug;
			++m_nIndexImageDebug;

			// Increment number of images showed
			++m_nNumImageShowed;
		}
		else
		{
			sprintf(pcImageName, "Image_%d_DEBUG_%d", m_nIndexImage, m_nIndexImageDebug-1);
			nIndexImage = m_nIndexImageDebug - 1;
		}

		cvNamedWindow(pcImageName, 0);
		cvResizeWindow(pcImageName, m_pWorkImageDebug->width, m_pWorkImageDebug->height);
		cvShowImage(pcImageName, m_pWorkImageDebug);

		cvWaitKey(nWaitTime);

		return nIndexImage;
	}

	bool CImage::CreateWorkImage()
	{
		if(m_pCurrentImage == NULL)
		{
			return false;
		}
		else
		{
			if(m_pWorkImage != NULL)
			{
				cvReleaseImage(&m_pWorkImage);
				m_pWorkImage = NULL;
			}
			if(m_pWorkImageDebug != NULL)
			{
				cvReleaseImage(&m_pWorkImageDebug);
				m_pWorkImageDebug = NULL;
			}

			// Always create image in gray scale
			m_pWorkImage = cvCreateImage(cvGetSize(m_pCurrentImage), m_pCurrentImage->depth, 1);
			cvZero(m_pWorkImage);
			cvCvtColor(m_pCurrentImage, m_pWorkImage, CV_BGR2GRAY);

			m_pWorkImageDebug = cvCreateImage(cvGetSize(m_pCurrentImage), m_pCurrentImage->depth, 1);
			cvZero(m_pWorkImageDebug);
			cvCopy(m_pWorkImage, m_pWorkImageDebug);
			return true;
		}
	}

	IplImage* CImage::GetTempImageFromAddress(char* pcImageAddress)
	{
		for(std::vector<IplImage*>::iterator i=m_vTempImagesVector.begin(); i<m_vTempImagesVector.end(); ++i)
		{
			if((*i)->imageData == pcImageAddress)
			{
				return *i;
			}
		}

		return NULL;
	}

	bool CImage::ReloadImageColor()
	{
		if(m_pCurrentImage == NULL)
		{
			return false;
		}

		cvZero(m_pCurrentImage);
		cvCvtColor(m_pWorkImage, m_pCurrentImage, CV_GRAY2BGR);
		return true;
	}

	bool CImage::ReloadDebugImage()
	{
		if(m_pWorkImageDebug == NULL)
		{
			return false;
		}

		cvCopy(m_pWorkImage, m_pWorkImageDebug);
		return true;
	}

	bool CImage::CleanDebugImage()
	{
		if(m_pWorkImageDebug == NULL)
		{
			return false;
		}
		cvZero(m_pWorkImageDebug);
		return true;
	}

	bool CImage::DeleteImageShowed(int nIndex)
	{
		char pcImageName[30] = "";
		sprintf(pcImageName, "Image_%d_%d", m_nIndexImage, nIndex);

		cvDestroyWindow(pcImageName);

		for(std::vector<int>::iterator i=m_vWindowIndex.begin(); i<m_vWindowIndex.end(); ++i)
		{
			if(*i == nIndex)
			{
				m_vWindowIndex.erase(i, i);
				break;
			}
		}

		// Reduce number of images showed
		--m_nNumImageShowed;
		return true;
	}

	bool CImage::DeleteImageDebugShowed(int nIndex)
	{
		char pcImageName[30] = "";
		sprintf(pcImageName, "Image_%d_DEBUG_%d", m_nIndexImage, nIndex);

		cvDestroyWindow(pcImageName);

		for(std::vector<int>::iterator i=m_vWindowIndexDebug.begin(); i<m_vWindowIndexDebug.end(); ++i)
		{
			if(*i == nIndex)
			{
				m_vWindowIndexDebug.erase(i, i);
				break;
			}
		}

		// Reduce number of images showed
		--m_nNumImageShowed;
		return true;
	}

	bool CImage::DestroyAllWindowsShowed()
	{
		char pcImageName[30] = "";
		int nIndex;

		// Delete image color showed
		for(std::vector<int>::iterator i=m_vWindowIndex.begin(); i<m_vWindowIndex.end(); ++i)
		{
			nIndex = *i;
			sprintf(pcImageName, "Image_%d_%d", m_nIndexImage, nIndex);
			cvDestroyWindow(pcImageName);
		}

		// Delete image used for debug
		for(std::vector<int>::iterator i=m_vWindowIndexDebug.begin(); i<m_vWindowIndexDebug.end(); ++i)
		{
			nIndex = *i;
			sprintf(pcImageName, "Image_%d_DEBUG_%d", m_nIndexImage, nIndex);
			cvDestroyWindow(pcImageName);
		}

		m_vWindowIndex.clear();
		m_nIndexImageColor = 0;

		m_vWindowIndexDebug.clear();
		m_nIndexImageDebug = 0;

		// Reset number of images showed
		m_nNumImageShowed = 0;

		return true;
	}

	bool CImage::CloneImage(CImage* pImage)
	{
		if(m_pCurrentImage == NULL)
		{
			return false;
		}
		pImage = new CImage();

		pImage->LoadImage(m_pCurrentImage);
		pImage->Reset();
		return true;
	}

	bool CImage::Reset()
	{
		m_nNumImageShowed = 0;
		m_vWindowIndex.clear();

		m_vWindowIndexDebug.clear();
		m_nIndexImageDebug = 0;
		return true;
	}

	bool CImage::GetProperty(int nPropertyId, void* pPropertyValue)
	{
		switch(nPropertyId)
		{
			case EIMAGE_WIDTH:
				*(int*)pPropertyValue = m_nImageWidth;
				break;

			case EIMAGE_WIDTHSTEP:
				*(int*)pPropertyValue = m_nImageWidthStep;
				break;

			case EIMAGE_HEIGHT:
				*(int*)pPropertyValue = m_nImageHeight;
				break;

			case EIMAGE_DATA_POINTER:
				*(unsigned char**)pPropertyValue = (unsigned char*)(m_pWorkImage->imageData);
				break;

			case EIMAGE_DEPTH:
				*(int*)pPropertyValue = m_pWorkImage->depth;
				break;

			default:
				return false;
		}

		return true;
	}

	void CImage::DrawPointDebug(int nX, int nY, unsigned char ucValue)
	{
	#ifdef MVL_GRAPHIC_ENABLE
		m_pWorkImageDebug->imageData[nY * m_nImageWidthStep + nX] = ucValue;
	#endif
	}

	int CImage::SampleHorizontalLine(int nXStart, int nYStart, int nNumSamples, unsigned char* pOutLine)
	{
		int nSamples = MIN(m_nImageWidth - nXStart, nNumSamples);

		for(int i=0; i<nSamples; i++)
		{
			pOutLine[i] = m_pWorkImage->imageData[nYStart * m_nImageWidthStep + nXStart + i];
		}

		return nSamples;
	}

	int CImage::SampleVerticalLine(int nXStart, int nYStart, int nNumSamples, unsigned char* pOutLine)
	{		
		int nSamples = MIN(m_nImageHeight - nYStart, nNumSamples);

		for(int i=0; i<nSamples; i++)
		{
			pOutLine[i] = m_pWorkImage->imageData[(nYStart + i) * m_nImageWidthStep + nXStart];
		}

		return nSamples;
	}

	int CImage::SampleBox(int nXStart, int nYStart, int nWidth, int nHeight, 
						  unsigned char* pOutBox, int* pBoxWidth, int* pBoxHeight)
	{
		int nBoxWidth = MIN(m_nImageWidth - nXStart, nWidth);
		int nBoxHeight = MIN(m_nImageHeight - nYStart, nHeight);
		int nNumSamples = 0;

		for(int j=nYStart; j<nYStart+nBoxHeight; j++)
		{
			for(int i=nXStart; i<nXStart+nBoxWidth; i++)
			{
				pOutBox[nNumSamples++] = m_pWorkImage->imageData[j*m_nImageWidthStep + i];
			}
		}

		*pBoxWidth = nBoxWidth;
		*pBoxHeight = nBoxHeight;
		return nNumSamples;
	}
}