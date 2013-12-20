#ifndef _MVL_INTERFACE_HEADER
#define _MVL_INTERFACE_HEADER


#ifdef __cplusplus    // If used by C++ code, 
extern "C" {          // we need to export the C interface
#endif

#ifdef _EXPORTING_DLL
	#define MVL_INTERFACE_EXPORT	__declspec(dllexport)
#else
	#define MVL_INTERFACE_EXPORT	//__declspec(dllimport)
#endif

#ifdef MVL_STANDARD_CONVENTION
	#define MVL_CALLING
#else
	#define MVL_CALLING __cdecl
#endif

/// @brief Property of Machine Vision Library
typedef enum _MVL_PROPERTY
{
	// Always the first one
	MVL_UNDEFINED_PROPERTY	= -1,
	
	// Select type of analysis
	MVL_SELECT_ANALYSIS,

	// MVL version
	MVL_VERSION_ID,

	// Image selected
	MVL_PROPERTY_IMAGE_ADDRESS,
	MVL_PROPERTY_GET_IMAGE_WIDTH,
	MVL_PROPERTY_GET_IMAGE_HEIGHT,

	// Used for results
	MVL_PROPERTY_RESULT_ANALIZER_ID,
	MVL_PROPERTY_RESULT_ANALIZER_NUM_POINTS,
	MVL_PROPERTY_RESULT_ANALIZER_NUM_BYTES,
	MVL_PROPERTY_RESULT_ANALIZER_DATA,

	// Always the last one
	MVL_NUMBER_OF_PROPERTY,

} MVL_PROPERTY;


/// @brief Analysis of Machine Vision Library
typedef enum _MVL_ANALYSIS
{
	// Always the first one
	MVL_UNDEFINED_ANALISYS	= -1,

	MVL_CANNY_ANALYSIS		= 0,
	MVL_GRADIENTE_ANALYSIS,
	MVL_SIFT_ANALYSIS,

	// Always the last one
	MVL_NUMBER_OF_ANALISYS,

} MVL_ANALYSIS;


/// @brief Defines value enabled and disabled
typedef enum _MVL_VALUE_TYPE
{
	MVL_VALUE_ENABLED	= 0,
	MVL_VALUE_DISABLED,

} MVL_VALUE_TYPE;


/// @brief MVL return type
typedef enum _MVL_RETURN_VALUE
{
	MVL_RETURN_SUCCESS	= 0,
	MVL_RETURN_FAILURE,

} MVL_RETURN_VALUE;


/// @brief Defines MVL error code
typedef enum _MVL_ERROR_CODE
{
	MVL_ERROR_UNDEFINED				= -1,
	MVL_ERROR_NOT_PRESENT			= 0,
	MVL_ERROR_PROPERTY_WRONG_VALUE,
	MVL_ERROR_ANALIZER_WORK,

} MVL_ERROR_CODE;

///////////////////////////// MVL API ///////////////////////////////

MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_Create(char* pImagePath);
MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_Destroy();

MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_SetProperty(int nParameterId, void* pValue, void* pValue2, void* pValue3);
MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_GetProperty(int nParameterId, void* pValue, void* pValue2, void* pValue3);
MVL_INTERFACE_EXPORT MVL_RETURN_VALUE MVL_CALLING MVL_Analyze();

MVL_INTERFACE_EXPORT MVL_ERROR_CODE MVL_CALLING MVL_GetLastError();

/////////////////////////////////////////////////////////////////////

#ifdef __cplusplus    // If used by C++ code, 
}					  // we need to export the C interface
#endif

#endif //_MVL_INTERFACE_HEADER