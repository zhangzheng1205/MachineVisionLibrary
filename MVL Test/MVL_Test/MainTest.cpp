#include "stdio.h"
#include "windows.h"
#include "MVL_API.h"


int main(int argc, char** argv)
{
	MVL_API libraryAPI;

	void* ptrFunc = NULL;
	char pcFuncName[20] = "";

	if(argc < 2)
	{
		return 0;
	}
	
	// Load dll of machine vision library
	if(libraryAPI.Initialize(argv[0]) == false)
	{
		return 0;
	}

	libraryAPI.SetProperty(MVL_API_CREATE, NULL, NULL, NULL);
	libraryAPI.SetProperty(MVL_API_DESTROY, NULL, NULL, NULL);
	libraryAPI.SetProperty(MVL_API_SET_PROPERTY, NULL, NULL, NULL);
	libraryAPI.SetProperty(MVL_API_GET_PROPERTY, NULL, NULL, NULL);
	libraryAPI.SetProperty(MVL_API_GET_LAST_ERROR, NULL, NULL, NULL);
	libraryAPI.SetProperty(MVL_API_ANALYZE, NULL, NULL, NULL);

	// Call API
	char* imagePath = argv[1];
	libraryAPI.CallMethod(MVL_API_CREATE, imagePath, NULL, NULL);
	libraryAPI.CallMethod(MVL_API_ANALYZE, NULL, NULL, NULL);

	libraryAPI.CallMethod(MVL_API_DESTROY, NULL, NULL, NULL);

	return true;
}