FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PhraseFluent.Website/PhraseFluent.Website.csproj", "PhraseFluent.Website/"]
COPY ["PhraseFluent.Service/PhraseFluent.Service.csproj", "PhraseFluent.Service/"]
COPY ["PhraseFluent.DataAccess/PhraseFluent.DataAccess.csproj", "PhraseFluent.DataAccess/"]
RUN dotnet restore "PhraseFluent.Website/PhraseFluent.Website.csproj"
COPY . .
WORKDIR "/src/PhraseFluent.Website"
RUN dotnet build "PhraseFluent.Website.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PhraseFluent.Website.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhraseFluent.Website.dll"]
