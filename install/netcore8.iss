[Code]
function IsDotNetCoreInstalled(const Version: String): Boolean;
var
  ResultCode: Integer;
  OutputFileName: String;
  DotNetCoreVersion: String;
  Lines: TStringList;
  i: Integer;
begin
  Result := False;
  OutputFileName := ExpandConstant('{tmp}\dotnet_list_runtimes.txt');
  Log('--OutputFileName is: ' + OutputFileName );
  Log('--Search  is: ' + 'Microsoft.WindowsDesktop.App '+Version );

  // Wykonanie polecenia dotnet --list-runtimes i zapisanie wyniku do pliku
  if Exec('cmd.exe', '/C dotnet --list-runtimes > "' + OutputFileName + '"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    // Wczytanie pliku tekstowego do listy
    Lines := TStringList.Create;
    try
      Lines.LoadFromFile(OutputFileName);

      // Sprawdzenie, czy linia zawiera odpowiednią wersję .NET Core
      for i := 0 to Lines.Count - 1 do
      begin
        if Pos('Microsoft.WindowsDesktop.App '+Version, Lines[i]) > 0 then
        begin
          Result := True;
          Break;
        end;
      end;
    finally
      Lines.Free;
    end;
  end;

  // Usunięcie pliku tymczasowego
  if FileExists(OutputFileName) then
  begin
    DeleteFile(OutputFileName);
  end;
end;

procedure Dependency_MyAddDotNet80Desktop;
begin
  if not IsDotNetCoreInstalled('8.0.11') then begin
    Dependency_Add('dotnet80desktop' + Dependency_ArchSuffix + '.exe',
      '/lcid ' + IntToStr(GetUILanguage) + ' /passive /norestart',
      '.NET Desktop Runtime 8.0.11' + Dependency_ArchTitle,
      Dependency_String(
        'https://download.visualstudio.microsoft.com/download/pr/a8d1a489-60d6-4e63-93ee-ab9c44d78b0d/5519f99ff50de6e096bb1d266dd0e667/dotnet-runtime-8.0.11-win-x86.exe',
        'https://download.visualstudio.microsoft.com/download/pr/53e9e41c-b362-4598-9985-45f989518016/53c5e1919ba2fe23273f2abaff65595b/dotnet-runtime-8.0.11-win-x64.exe'),
      '', False, False);
  end;
end;
