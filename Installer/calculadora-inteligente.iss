#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif

[Setup]
AppId={{E52B4B74-8D2A-4864-8C5A-708A5DF1E5D3}
AppName=MultiCálculos
AppVersion={#AppVersion}
AppPublisher=Cristiano Pirolli
AppPublisherURL=https://github.com/CristianoPirolli
AppSupportURL=https://github.com/CristianoPirolli
AppUpdatesURL=https://github.com/CristianoPirolli
DefaultDirName={autopf}\MultiCalculos
DefaultGroupName=MultiCálculos
OutputDir=build
OutputBaseFilename=MultiCalculos-Setup-{#AppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=classic
SetupIconFile=..\UI\image\calculator_icon.ico
PrivilegesRequired=lowest
DisableProgramGroupPage=yes
ShowLanguageDialog=no

[Files]
Source: "build\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\MultiCálculos"; Filename: "{app}\MultiCalculos.exe"; IconFilename: "{app}\MultiCalculos.exe"
Name: "{autodesktop}\MultiCálculos"; Filename: "{app}\MultiCalculos.exe"; IconFilename: "{app}\MultiCalculos.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na área de trabalho"; GroupDescription: "Opções extras:"

[Run]
Filename: "{app}\MultiCalculos.exe"; Description: "Abrir aplicativo"; Flags: nowait postinstall skipifsilent
