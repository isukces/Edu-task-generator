; �����
; #define dotnet_Passive
; #define dontuse_dotnetfx45
; #define use_dotnetfx461
#include "scripts\xInnoDependencyInstaller\CodeDependencies.iss"
#include "netcore8.iss"

[Languages]
Name: en; MessagesFile: compiler:Default.isl
Name: de; MessagesFile: compiler:Languages\German.isl
Name: pl; MessagesFile: compiler:Languages\Polish.isl
; Name: nl; MessagesFile: compiler:Languages\Dutch.isl
; Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
; Name: "finnish"; MessagesFile: "compiler:Languages\Finnish.isl"

[Setup]
OutputDir=Out
OutputBaseFilename=XEducation1_24_1120_2
VersionInfoVersion=1.24.1120.2
VersionInfoCompany=iSukces
VersionInfoDescription=XEducation
VersionInfoCopyright=iSukces
VersionInfoProductName=XEducation
VersionInfoProductVersion=1.24.1120.2
SolidCompression=true
PrivilegesRequired=none
AppID={{26D7D23B-1137-4A14-B33B-EE29AA3284E0}
AppCopyright=iSukces
AppName=XEducation
AppVersion=1.24.1120.2
DefaultDirName={pf}\iSukces\XEducation
DefaultGroupName=XEducation
InternalCompressLevel=ultra

UninstallDisplayIcon={app}\XEducation.exe
UninstallDisplayName=XEducation
AppUpdatesURL=http://www.iSukces.pl/
AppPublisher=iSukces

DisableWelcomePage=True
DisableFinishedPage=True
DisableProgramGroupPage=auto
DisableDirPage=auto
DisableReadyPage=True
DisableReadyMemo=True
ShowTasksTreeLines=True
AlwaysShowGroupOnReadyPage=True
AlwaysShowDirOnReadyPage=True

[Files]
Source: "..\XEducation\bin\Release\net8.0-windows\iSukces.Mathematics.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\iSukces.Translation.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\JetBrains.Annotations.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\MahApps.Metro.IconPacks.Core.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\MahApps.Metro.IconPacks.Material.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\MahApps.Metro.IconPacks.PhosphorIcons.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\MahApps.Metro.IconPacks.VaadinIcons.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\XEducation.deps.json"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\XEducation.dll"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\XEducation.exe"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "..\XEducation\bin\Release\net8.0-windows\XEducation.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion replacesameversion
Source: "scripts\xInnoDependencyInstaller\netcorecheck.exe"; Flags: dontcopy noencryption
Source: "scripts\xInnoDependencyInstaller\netcorecheck_x64.exe"; Flags: dontcopy noencryption

[InstallDelete]

[ThirdParty]
; CompileLogFile=install-log.txt

[Icons]
Name: "{group}\XEducation"; Filename: "{app}\XEducation.exe"; WorkingDir: "{app}"; Flags: runmaximized; IconFilename: "{app}\XEducation.exe"; IconIndex: 0
Name: "{userdesktop}\XEducation"; Filename: "{app}\XEducation.exe"; WorkingDir: "{app}\"; Flags: runmaximized; IconIndex: 0; Comment: "XEducation"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\XEducation"; Filename: "{app}\XEducation.exe"; WorkingDir: "{app}\"; Flags: runmaximized; IconIndex: 0; Comment: "XEducation"

[Registry]
 
[Run]
; Filename: "taskkill"; Parameters: "/im ""conexx.total.exe"" /f"; Flags: runhidden
#include "scripts\products.iss"
#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"

#ifdef use_dotnetfx40
#include "scripts\products\dotnetfx40full.iss"
#endif
#ifdef use_dotnetfx45
#include "scripts\products\dotnetfxversion.iss"
#include "scripts\products\dotnetfx45full.iss"
#endif

#ifdef use_dotnetfx461
#include "scripts\products\dotnetfxversion.iss"
#include "scripts\products\dotnetfx461full.iss"
#endif

#include "scripts\products\directx9.iss"

[Code]

function InitializeSetup: Boolean;
begin
    Dependency_MyAddDotNet80Desktop();
    Result := True;
end;