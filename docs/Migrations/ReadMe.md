# Create migrations


Those commands shloud be executed from 'src/' folder.


`dotnet ef migrations add <Migration_name> -p Infrastructure/Infrastructure.csproj -s Api/Api.csproj -o Database/Migrations`


`dotnet ef database update -p Api/Api.csproj`

