## Installing the tools

`dotnet ef` can be installed as either a global or local tool. Most developers prefer installing dotnet ef as a global tool using the following command:

```
dotnet tool install --global dotnet-ef
```


Update the tool using the following command:
```
dotnet tool update --global dotnet-ef
```

Before you can use the tools on a specific project, you'll need to add the Microsoft.EntityFrameworkCore.Design package to it.
```
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Run the following commands to verify that EF Core CLI tools are correctly installed:
```
dotnet ef
```

Open command prompt and navigate to the root of this project.

To create initial migration modify settings `Platform.DatabaseType` to `MsSql` and use command:
```
dotnet ef migrations add "MsSqlInitial"  -o Migrations/MsSql
```

To generate script use the following commmand:
```
dotnet ef migrations script -o ".\Migrations\MsSql\Scripts\V0001_Initial_.sql" 
```

The code generated in UTF-8 with BOM that is not compatable with the workbanch. Convert to UTF-8 with the following powershell command:
```
Get-Content ".\Migrations\MsSql\Scripts\V0001_Initial_.sql" | Set-Content -Encoding Default ".\Migrations\MsSql\Scripts\V0001_Initial.sql"
DEL ".\Migrations\MsSql\Scripts\V0001_Initial_.sql"
```

