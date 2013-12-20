#ifndef _MVL_IMAGE_HEADER_
#define _MVL_IMAGE_HEADER_

#include "OpenCVHeader.h"
#include "stdio.h"
#include <vector>

namespace MVL
{
	/// @brief Defines image properties
	typedef enum _IMAGE_PROPERTY
	{
		EIMAGE_WIDTH	= 0,
		EIMAGE_WIDTHSTEP,
		EIMAGE_HEIGHT,
		EIMAGE_DATA_POINTER,
		EIMAGE_DEPTH

	} IMAGE_PROPERTY;

	class CImage
	{
	private:

		IplImage* m_pCurrentImage;
		IplImage* m_pWorkImage;
		IplImage* m_pWorkImageDebug;
		
		int m_nNumImageShowed;
		int m_nIndexImage; // Index used by instance of CImage class
		
		std::vector<int> m_vWindowIndex; ///< Manage number of windows opened for one image
		int m_nIndexImageColor;

		std::vector<int> m_vWindowIndexDebug; ///< Manage number of windows opened for debug image
		int m_nIndexImageDebug;

		static int s_nNumImageIndex;///< When a new CImage is created this index is incremented
		static std::vector<int> s_vIndexImagesHistory;

		int m_NumImagesTemporaryCreated; // Number of temp images created
		std::vector<IplImage*> m_vTempImagesVector;

		int m_nImageWidth;
		int m_nImageWidthStep;
		int m_nImageHeight;

		bool CreateWorkImage();
		IplImage* GetTempImageFromAddress(char* pcImageAddress);

	public:

		CImage();
		CImage(const CImage& image){};
		~CImage();

		bool Reset();
		bool GetProperty(int nPropertyId, void* pPropertyValue);

		// An object can make copy of itself
		bool CloneImage(CImage* pImage);

		// Used to load images
		bool LoadImage(const char* pImagePath);
		bool LoadImage(IplImage* pImage);

		// Used by analizers to work on images
		unsigned char* CreateTempImageUC(int nWidth, int nHeight, int* pnWidthStep);
		int* CreateTempImage32S(int nWidth, int nHeight, int* pnWidthStep);

		template<typename Type> bool DestroyTempImage(Type* pucImageAddress);

		// Used to show images
		int ShowImage(int nWaitTime, bool bNewImage);
		int ShowImageDebug(int nWaitTime, bool bNewImage);

		// Reload image like initial
		bool ReloadImageColor();
		bool ReloadDebugImage();
		bool CleanDebugImage();

		// Used to destroy images
		bool DeleteImageShowed(int nIndex);
		bool DeleteImageDebugShowed(int nIndex);
		bool DestroyAllWindowsShowed();

		// Draw functions
		void DrawPointDebug(int nX, int nY, unsigned char ucValue);

		// Return number of points sampled
		int SampleHorizontalLine(int nXStart, int nYStart, int nNumSamples, unsigned char* pOutLine);
		int SampleVerticalLine(int nXStart, int nYStart, int nNumSamples, unsigned char* pOutLine);

		// It's the corner upper left
		int SampleBox(int nXStart, int nYStart, int nWidth, int nHeight,
					  unsigned char* pOutBox, int* pBoxWidth, int* pBoxHeight);

		template<typename Type> 
		bool ExecuteNonMaximaSuppressionImage(Type* pucImageAddress, int nWidth, int nHeight);
	};

	template<typename Type>
	bool CImage::DestroyTempImage(Type* pucImageAddress)
	{
		if(m_NumImagesTemporaryCreated <= 0)
		{
			return false;
		}

		for(std::vector<IplImage*>::iterator i=m_vTempImagesVector.begin(); i<m_vTempImagesVector.end(); ++i)
		{
			if((*i)->imageData == (char*)pucImageAddress)
			{
				cvReleaseImage(&(*i));
				m_vTempImagesVector.erase(i);
				--m_NumImagesTemporaryCreated;
				return true;
			}
		}

		return false;
	}

	template<typename Type>
	bool CImage::ExecuteNonMaximaSuppressionImage(Type* pucImageAddress, int nWidth, int nHeight)
	{
		IplImage* pImage = GetTempImageFromAddress((char*)pucImageAddress);

		if(pImage == NULL)
		{
			return false;
		}

		IplImage* pImageNMS = NULL;

		pImageNMS = cvCloneImage(pImage);
		
		assert(pImageNMS != NULL);
		cvZero(pImageNMS);

		bool bFirstVal = false;
		bool bSign = false; // false it means negative, otherwise positive
		int nStartX, nStartY;
		int nXCoord, nYCoord;
		Type value;

		for(int j=0; (j<pImage->height) && (bFirstVal==false); j++)
		{
			for(int i=0; (i<pImage->width) && (bFirstVal==false); i++)
			{
				if(((Type*)(pImage->imageData))[j*pImage->widthStep + i] != (Type)0)
				{
					if(((Type*)(pImage->imageData))[j*pImage->widthStep + i] > (Type)0)
					{
						bSign = true;
					}
					else
					{
						bSign = false;
					}
					nStartX = nXCoord = i;
					nStartY = nYCoord = j;
					value = ((Type*)(pImage->imageData))[j*pImage->widthStep + i];
					bFirstVal = true;
					break;
				}
			}
		}

		if(bFirstVal == false)
		{
			cvReleaseImage(&pImageNMS);
			return false;
		}
		
		// Non maxima suppression on first line not zero
		for(int i=nStartX+1; i<pImage->width; i++)
		{
			if(bSign == true)
			{
				if(((Type*)(pImage->imageData))[nStartY*pImage->widthStep + i] < (Type)0)
				{
					((Type*)(pImageNMS->imageData))[nStartY*pImage->widthStep + nXCoord] = value;
					bSign = false;
					nXCoord = i;
				}
				else
				{
					if(((Type*)(pImage->imageData))[nStartY*pImage->widthStep + i] > value)
					{
						value = ((Type*)(pImage->imageData))[nStartY*pImage->widthStep + i];
						nXCoord = i;
					}
				}
			}
			else
			{
				if(((Type*)(pImage->imageData))[nStartY*pImage->widthStep + i] > (Type)0)
				{
					((Type*)(pImageNMS->imageData))[nStartY*pImage->widthStep + nXCoord] = value;
					bSign = true;
					nXCoord = i;
				}
				else
				{
					if(((Type*)(pImage->imageData))[nStartY*pImage->widthStep + i] < value)
					{
						value = ((Type*)(pImage->imageData))[nStartY*pImage->widthStep + i];
						nXCoord = i;
					}
				}
			}
		}

		for(int j=nStartY+1; j<pImage->height; j++)
		{
			for(int i=0; i<pImage->width; i++)
			{
				if(bSign == true)
				{
					if(((Type*)(pImage->imageData))[j*pImage->widthStep + i] < (Type)0)
					{
						((Type*)(pImageNMS->imageData))[nYCoord*pImage->widthStep + nXCoord] = value;
						bSign = false;
						nXCoord = i;
						nYCoord = j;
					}
					else
					{
						if(((Type*)(pImage->imageData))[j*pImage->widthStep + i] > value)
						{
							value = ((Type*)(pImage->imageData))[j*pImage->widthStep + i];
							nXCoord = i;
							nYCoord = j;
						}
					}
				}
				else
				{
					if(((Type*)(pImage->imageData))[j*pImage->widthStep + i] > (Type)0)
					{
						((Type*)(pImageNMS->imageData))[nYCoord*pImage->widthStep + nXCoord] = value;
						bSign = true;
						nXCoord = i;
						nYCoord = j;
					}
					else
					{
						if(((Type*)(pImage->imageData))[j*pImage->widthStep + i] < value)
						{
							value = ((Type*)(pImage->imageData))[j*pImage->widthStep + i];
							nXCoord = i;
							nYCoord = j;
						}
					}
				}
			}
		}

		// Copy image Non-Maxima suppressed to image passed by parameter
		cvZero(pImage);
		cvCopy(pImageNMS, pImage);
		cvReleaseImage(&pImage);

		return true;
	}
};

#endif //_MVL_IMAGE_HEADER_