FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroservicesExample.Auth/MicroservicesExample.Auth.csproj", "MicroservicesExample.Auth/"]
RUN dotnet restore "MicroservicesExample.Auth/MicroservicesExample.Auth.csproj" 
COPY . .
WORKDIR "/src/MicroservicesExample.Auth"
RUN dotnet build "MicroservicesExample.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroservicesExample.Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroservicesExample.Auth.dll"]