
#define AppName "TelemedAudio"
#define ProcessName "AudioProcessManagement"
#define AppVer "v1.3.1"
#define Arch "x86"

[Setup]
AppName={#AppName}
AppPublisher=Telemed Technology
AppPublisherURL=http://www.telemed.com.tr/
AppVersion={#AppVer}
DefaultDirName=C:\TelemedAudio
DefaultGroupName={#AppName}
UninstallDisplayIcon={app}\{#AppName}
OutputDir=C:\Users\telemed-Dell\Desktop\AudioInputOutputManager v1.3.1 - Setup\Output
OutputBaseFilename={#AppName}_{#AppVer}_{#Arch}
Compression=lzma
SolidCompression=yes

[Files]
Source: "C:\Users\telemed-Dell\Desktop\AudioInputOutputManager v1.3.1 - Setup\Audio.ini"; DestDir: "{app}"
Source: "C:\Users\telemed-Dell\Desktop\AudioInputOutputManager v1.3.1 - Setup\*.dll"; DestDir: "{app}"
Source: "C:\Users\telemed-Dell\Desktop\AudioInputOutputManager v1.3.1 - Setup\*.exe"; DestDir: "{app}"

[Run]
Filename: "{app}\{#ProcessName}.exe"; Description: "Launch application"; Flags: postinstall nowait skipifsilent

[Icons]
Name: {group}\{#AppName}; Filename: {app}\{#ProcessName}.exe; WorkingDir: {app};
Name: {group}\{cm:UninstallProgram,{#AppName}}; Filename: {uninstallexe}

[Registry]
Root: HKLM; Subkey: "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "{#AppName}"; ValueData: "{app}\{#ProcessName}.exe"