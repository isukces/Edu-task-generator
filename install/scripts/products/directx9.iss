
[CustomMessages]
directx_title=DirectX End-User Runtimes (June 2010)
directx_size=95.6MB
 
; http://www.microsoft.com/globaldev/reference/lcid-all.mspx
; en.dotnetfx45full_lcid='' 

[Code]

const
    directx_url = 'http://download.microsoft.com/download/8/4/A/84A35BF1-DAFE-4AE8-82AF-AD2AE20B6B14/directx_Jun2010_redist.exe';


procedure DecodeVersion( verstr: String; var verint: array of Integer );
var
  i,p: Integer; s: string;
begin
  // initialize array
  verint := [0,0,0,0];
  i := 0;
  while ( (Length(verstr) > 0) and (i < 4) ) do
  begin
    p := pos('.', verstr);
    if p > 0 then
    begin
      if p = 1 then s:= '0' else s:= Copy( verstr, 1, p - 1 );
      verint[i] := StrToInt(s);
      i := i + 1;
      verstr := Copy( verstr, p+1, Length(verstr));
    end
    else
    begin
      verint[i] := StrToInt( verstr );
      verstr := '';
    end;
  end;

end;

// This function compares version string
// return -1 if ver1 < ver2
// return  0 if ver1 = ver2
// return  1 if ver1 > ver2
function CompareDirectXVersion( ver1, ver2: String ) : Integer;
var
  verint1, verint2: array of Integer;
  i: integer;
begin

  SetArrayLength( verint1, 4 );
  DecodeVersion( ver1, verint1 );

  SetArrayLength( verint2, 4 );
  DecodeVersion( ver2, verint2 );

  Result := 0; i := 0;
  while ( (Result = 0) and ( i < 4 ) ) do
  begin
    if verint1[i] > verint2[i] then
      Result := 1
    else
      if verint1[i] < verint2[i] then
        Result := -1
      else
        Result := 0;

    i := i + 1;
  end;

end;

// DirectX version is stored in registry as 4.majorversion.minorversion
// DirectX 8.0 is 4.8.0
// DirectX 8.1 is 4.8.1
// DirectX 9.0 is 4.9.0

function GetDirectXVersion(): String;
var
  sVersion:  String;
begin
  sVersion := '';

  RegQueryStringValue( HKLM, 'SOFTWARE\Microsoft\DirectX', 'Version', sVersion );

  Log('detected DirectX version: ' + sVersion);
  Result := sVersion;
end;


function checkDirectX(): boolean;
begin
  Result := false;
  exit;
  // in this case program needs at least directx 9.0
  if CompareDirectXVersion( GetDirectXVersion(), '4.9') < 0 then
  begin
       Result := true;
  end
  else
  begin
    Result := false;
   end;

end;



procedure directX();
begin
  // in this case program needs at least directx 9.0
  if CompareDirectXVersion( GetDirectXVersion(), '4.09.0') < 0 then
  begin
      Log('Update DirectX');
                AddProduct('directx_Jun2010_redist.exe',
                    '/t:' + ExpandConstant('{tmp}\DirectX') + ' /q /c',
                    CustomMessage('directx_title'),
                    CustomMessage('directx_size'),
                    directx_url,
                    false, false);

  end;
end;

