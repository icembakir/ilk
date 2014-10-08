#define AUDIOSESSIONMANAGER_API __declspec(dllexport)


enum DeviceType{MICROPHONE,APPLICATION};

extern "C" __declspec(dllexport) bool setMute(DeviceType devType);
extern "C" __declspec(dllexport) bool setActive(DeviceType devType);
extern "C" __declspec(dllexport) bool setApplicationsMute();
extern "C" __declspec(dllexport) bool setApplicationsActive();
extern "C" __declspec(dllexport) bool setMicrophoneMute();
extern "C" __declspec(dllexport) bool setMicrophoneActive();
extern "C" __declspec(dllexport) bool volumeUp();
extern "C" __declspec(dllexport) bool volumeDown();