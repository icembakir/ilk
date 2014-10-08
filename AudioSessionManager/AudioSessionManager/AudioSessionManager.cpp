#include <cstring>
#include <Audiopolicy.h>
#include <Mmdeviceapi.h>
#include <Objbase.h>
#include <iostream>
#include <endpointvolume.h>
#include "AudioSessionManager.h"
using namespace std;

/*
static const CLSID CLSID_MMDeviceEnumerator = __uuidof(MMDeviceEnumerator);
static const IID IID_IMMDeviceEnumerator = __uuidof(IMMDeviceEnumerator);
static const IID IID_IAudioSessionManager2 = __uuidof(IAudioSessionManager2);
*/
#define CHECK_ERROR_SET_MUTE(result,hres) \
		if (FAILED(hres)){ result=false; goto Exit_Set_Mute;}

#define CHECK_ERROR_SET_ACTIVE(result,hres) \
		if (FAILED(hres)){ result=false; goto Exit_Set_Active;}

#define CHECK_ERROR_STATE_VOLUMEUP(result,hres) \
		if(FAILED(hres)){ result=false; goto Exit_VolumeUp;}

#define CHECK_ERROR_STATE_VOLUMEDOWN(result,hres) \
		if(FAILED(hres)){ result=false; goto Exit_VolumeDown;}

//static map<LPWSTR,float> audioValues;

bool setMute(DeviceType devType){
	bool result=true;

	HRESULT hr = S_OK;
    IMMDeviceEnumerator *pEnumerator = NULL;
    IMMDevice *pEndpoint = NULL;
	IAudioSessionManager2 *sessionManager;
	IAudioSessionEnumerator *sessionList;
	IAudioSessionControl *sessionControl;
	IAudioSessionControl2 *sessionControl2;
	ISimpleAudioVolume *simpleAudioVolume;
	LPWSTR pswSession = NULL;
	char sessionControlName[100];

	hr=CoInitialize(NULL);
	//CHECK_ERROR_SET_MUTE(result,hr);

	hr = CoCreateInstance( __uuidof(MMDeviceEnumerator), NULL, CLSCTX_ALL, __uuidof(IMMDeviceEnumerator),(void**)&pEnumerator);
	CHECK_ERROR_SET_MUTE(result,hr);

	hr=pEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &pEndpoint);
	CHECK_ERROR_SET_MUTE(result,hr);

	hr=pEndpoint->Activate(__uuidof(IAudioSessionManager2),CLSCTX_ALL,NULL,(void**)&sessionManager);
	CHECK_ERROR_SET_MUTE(result,hr);

	hr=sessionManager->GetSessionEnumerator(&sessionList);
	CHECK_ERROR_SET_MUTE(result,hr);

	int count;
	hr=sessionList->GetCount(&count);
	CHECK_ERROR_SET_MUTE(result,hr);

	for(int i=0;i<count;i++){
		hr=sessionList->GetSession(i,&sessionControl);
		CHECK_ERROR_SET_MUTE(result,hr);

		hr=sessionControl->GetDisplayName(&pswSession);
		CHECK_ERROR_SET_MUTE(result,hr);
		//wprintf_s(L"Session Name:%s+++\n", pswSession);
		
		wcstombs(sessionControlName,pswSession,100);

		hr=sessionControl->QueryInterface(__uuidof(IAudioSessionControl2), (void**)&sessionControl2);
		CHECK_ERROR_SET_MUTE(result,hr);

		hr=sessionControl2->QueryInterface(__uuidof(ISimpleAudioVolume), (void**)&simpleAudioVolume);
		CHECK_ERROR_SET_MUTE(result,hr);

		if(devType==APPLICATION && strncmp(sessionControlName,"@%SystemRoot%",strlen("@%SystemRoot%"))!=0 && strncmp(sessionControlName,"Mikrofon",strlen("Mikrofon"))!=0 && strncmp(sessionControlName,"Microphone",strlen("Microphone"))!=0 && strncmp(sessionControlName,"Line-In",strlen("Line-In"))!=0 && strncmp(sessionControlName,"Line In",strlen("Line In"))!=0 && strncmp(sessionControlName,"Hat Gir",strlen("Hat Gir"))!=0){
			//simpleAudioVolume->SetMute(true,NULL);
			//float value;
			//simpleAudioVolume->GetMasterVolume(&value);
			//audioValues[pswSession]=value;
			hr=simpleAudioVolume->SetMute(true,NULL);
			CHECK_ERROR_SET_MUTE(result,hr);
		}else if(devType==MICROPHONE && (strncmp(sessionControlName,"Mikrofon",strlen("Mikrofon"))==0 || strncmp(sessionControlName,"Microphone",strlen("Microphone"))==0)){
			hr=simpleAudioVolume->SetMute(true,NULL);
			CHECK_ERROR_SET_MUTE(result,hr);
			CoTaskMemFree(pswSession);
			simpleAudioVolume->Release();
			sessionControl2->Release();
			sessionControl->Release();
			break;
		}
		CoTaskMemFree(pswSession);
		strcpy(sessionControlName,"");
		simpleAudioVolume->Release();
		sessionControl2->Release();
		sessionControl->Release();
	}

Exit_Set_Mute:
	//CoTaskMemFree(pswSession);
	pEndpoint->Release();
	pEnumerator->Release();
	sessionManager->Release();
	sessionList->Release();

	CoUninitialize();

	return result;
}

bool setActive(DeviceType devType){
	bool result=true;

	HRESULT hr = S_OK;
    IMMDeviceEnumerator *pEnumerator = NULL;
    IMMDevice *pEndpoint = NULL;
	IAudioSessionManager2 *sessionManager;
	IAudioSessionEnumerator *sessionList;
	IAudioSessionControl *sessionControl;
	IAudioSessionControl2 *sessionControl2;
	ISimpleAudioVolume *simpleAudioVolume;
	LPWSTR pswSession = NULL;
	char sessionControlName[100];

	hr=CoInitialize(NULL);
	//CHECK_ERROR_SET_ACTIVE(result,hr);

	hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_ALL, __uuidof(IMMDeviceEnumerator),(void**)&pEnumerator);
	CHECK_ERROR_SET_ACTIVE(result,hr);

	hr=pEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &pEndpoint);
	CHECK_ERROR_SET_ACTIVE(result,hr);

	hr=pEndpoint->Activate(__uuidof(IAudioSessionManager2),CLSCTX_ALL,NULL,(void**)&sessionManager);
	CHECK_ERROR_SET_ACTIVE(result,hr);

	hr=sessionManager->GetSessionEnumerator(&sessionList);
	CHECK_ERROR_SET_ACTIVE(result,hr);

	int count;
	hr=sessionList->GetCount(&count);
	CHECK_ERROR_SET_ACTIVE(result,hr);
	
	for(int i=0;i<count;i++){
		hr=sessionList->GetSession(i,&sessionControl);
		CHECK_ERROR_SET_ACTIVE(result,hr);

		hr=sessionControl->GetDisplayName(&pswSession);
		CHECK_ERROR_SET_ACTIVE(result,hr);
		//wprintf_s(L"Session Name:%s+++\n", pswSession);
		
		wcstombs(sessionControlName,pswSession,100);

		hr=sessionControl->QueryInterface(__uuidof(IAudioSessionControl2), (void**)&sessionControl2);
		CHECK_ERROR_SET_ACTIVE(result,hr);

		hr=sessionControl2->QueryInterface(__uuidof(ISimpleAudioVolume), (void**)&simpleAudioVolume);
		CHECK_ERROR_SET_ACTIVE(result,hr);

		if(devType==APPLICATION && strncmp(sessionControlName,"@%SystemRoot%",strlen("@%SystemRoot%"))!=0 && strncmp(sessionControlName,"Mikrofon",strlen("Mikrofon"))!=0 && strncmp(sessionControlName,"Microphone",strlen("Microphone"))!=0 && strncmp(sessionControlName,"Line-In",strlen("Line-In"))!=0 && strncmp(sessionControlName,"Line In",strlen("Line In"))!=0 && strncmp(sessionControlName,"Hat Gir",strlen("Hat Gir"))!=0){
			hr=simpleAudioVolume->SetMute(false,NULL);
			CHECK_ERROR_SET_ACTIVE(result,hr);
		}else if(devType==MICROPHONE && ( strncmp(sessionControlName,"Mikrofon",strlen("Mikrofon")) == 0 || strncmp(sessionControlName,"Microphone",strlen("Microphone")) ==0 )){
			hr=simpleAudioVolume->SetMute(false,NULL);
			CHECK_ERROR_SET_ACTIVE(result,hr);
			CoTaskMemFree(pswSession);
			simpleAudioVolume->Release();
			sessionControl2->Release();
			sessionControl->Release();
			break;
		}

		strcpy(sessionControlName,"");
		CoTaskMemFree(pswSession);
		simpleAudioVolume->Release();
		sessionControl2->Release();
		sessionControl->Release();
	}

	//audioValues.clear();

Exit_Set_Active:
	//CoTaskMemFree(pswSession);
	pEndpoint->Release();
	pEnumerator->Release();
	sessionManager->Release();
	sessionList->Release();

	CoUninitialize();

	return result;
}

bool setApplicationsMute(){
	return setMute(APPLICATION);
}

bool setApplicationsActive(){
	return setActive(APPLICATION);
}

bool setMicrophoneMute(){
	return setMute(MICROPHONE);
}

bool setMicrophoneActive(){
	return setActive(MICROPHONE);
}

bool volumeUp()
{
	bool result = true;
	HRESULT hr;
	IMMDevice *defaultDevice = NULL;
	IAudioEndpointVolume *endpointVolume = NULL;

	CoInitialize(NULL);
	IMMDeviceEnumerator *deviceEnumerator = NULL;
	hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
	CHECK_ERROR_STATE_VOLUMEUP(result, hr);

	hr = deviceEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &defaultDevice);
	CHECK_ERROR_STATE_VOLUMEUP(result, hr);
	deviceEnumerator->Release();
	deviceEnumerator = NULL;
	
	hr = defaultDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);
	CHECK_ERROR_STATE_VOLUMEUP(result, hr);
	defaultDevice->Release();
	defaultDevice = NULL; 

	float currentVolume = 0;
	hr = endpointVolume->GetMasterVolumeLevelScalar(&currentVolume);
	CHECK_ERROR_STATE_VOLUMEUP(result, hr);

	currentVolume = (currentVolume > 0.95f) ? 1.0f : currentVolume + 0.05f;
	hr = endpointVolume->SetMasterVolumeLevelScalar(currentVolume, NULL);
	CHECK_ERROR_STATE_VOLUMEUP(result, hr);

Exit_VolumeUp:
	if(endpointVolume != NULL)
		endpointVolume->Release();
	CoUninitialize();
	return result;
}

bool volumeDown()
{
	bool result = true;
	HRESULT hr;
	IMMDevice *defaultDevice = NULL;
	IAudioEndpointVolume *endpointVolume = NULL;

	CoInitialize(NULL);
	IMMDeviceEnumerator *deviceEnumerator = NULL;
	hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
	CHECK_ERROR_STATE_VOLUMEDOWN(result, hr);

	hr = deviceEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &defaultDevice);
	CHECK_ERROR_STATE_VOLUMEDOWN(result, hr);
	deviceEnumerator->Release();
	deviceEnumerator = NULL;
	
	hr = defaultDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);
	CHECK_ERROR_STATE_VOLUMEDOWN(result, hr);
	defaultDevice->Release();
	defaultDevice = NULL; 

	float currentVolume = 0;
	hr = endpointVolume->GetMasterVolumeLevelScalar(&currentVolume);
	CHECK_ERROR_STATE_VOLUMEDOWN(result, hr);

	currentVolume = (currentVolume < 0.05f) ? 0.0f : currentVolume - 0.05f;
	hr = endpointVolume->SetMasterVolumeLevelScalar(currentVolume, NULL);
	CHECK_ERROR_STATE_VOLUMEDOWN(result, hr);

Exit_VolumeDown:
	if(endpointVolume != NULL)
		endpointVolume->Release();
	CoUninitialize();
	return result;
}