FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroservicesExample.Order/MicroservicesExample.Order.csproj", "MicroservicesExample.Order/"]
RUN dotnet restore "MicroservicesExample.Order/MicroservicesExample.Order.csproj" 
COPY . .
WORKDIR "/src/MicroservicesExample.Order"
RUN dotnet build "MicroservicesExample.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroservicesExample.Order.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroservicesExample.Order.dll"]