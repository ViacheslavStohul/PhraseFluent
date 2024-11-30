## Installing the tools

To create initial migration use command:
```
dotnet ef migrations add "MySqlInitial"  -o Migrations
```

To generate script use the following commmand:
```
dotnet ef migrations script -o ".\Migrations\Scripts\V0001_Initial_.sql" 
```

The code generated in UTF-8 with BOM that is not compatable with the workbanch. Convert to UTF-8 with the following powershell command:
```
Get-Content ".\Migrations\MsSql\Scripts\V0001_Initial_.sql" | Set-Content -Encoding Default ".\Migrations\Scripts\V0001_Initial.sql"
DEL ".\Migrations\MsSql\Scripts\V0001_Initial_.sql"
```

