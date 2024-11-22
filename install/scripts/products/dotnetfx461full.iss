// requires Windows Vista SP2 (x86 and x64), Windows 7 SP1 (x86 and x64), Windows Server 2008 R2 SP1 (x64), Windows Server 2008 SP2 (x86 and x64)
// requires Windows Installer 3.1
// requires Internet Explorer 5.01
// WARNING: express setup (downloads and installs the components depending on your OS) if you want to deploy it on cd or network download the full bootsrapper on website below
// http://www.microsoft.com/downloads/en/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992
 
[CustomMessages]
dotnetfx461full_title=.NET Framework 4.6.1 Full
 
dotnetfx461full_size=3 MB - 197 MB
 
;http://www.microsoft.com/globaldev/reference/lcid-all.mspx
en.dotnetfx45full_lcid='' 

[Code]
const
  dotnetfx461full_url = 'http://download.microsoft.com/download/E/4/1/E4173890-A24A-4936-9FC9-AF930FE3FA40/NDP461-KB3102436-x86-x64-AllOS-ENU.exe';
 
procedure dotnetfx461full();
var version1: integer;
begin
  Log('Checking framework version');
  version1 := netfxspversion(NetFx4x, '');
  Log('Framework version: ' + IntToStr(version1));
  if ( not netfxinstalled(NetFx4x, '') or (version1 < 61)) then
      AddProduct('NDP461-KB3102436-x86-x64-AllOS-ENU.exe',
        '/passive /norestart',
        CustomMessage('dotnetfx461full_title'),
        CustomMessage('dotnetfx461full_size'),
        dotnetfx461full_url,
      false, false);    
end;



// procedure dotnetfx46(MinVersion: integer);
// begin
// 	if (not netfxinstalled(NetFx4x, '') or (netfxspversion(NetFx4x, '') < MinVersion)) then
// 		AddProduct('dotnetfx46.exe',
// 			CustomMessage('dotnetfx46_lcid') + ' /passive /norestart',
// 			CustomMessage('dotnetfx46_title'),
// 			CustomMessage('dotnetfx46_size'),
// 			dotnetfx461full_url,
// 			false, false);
// end;
