FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroservicesExample.Product/MicroservicesExample.Product.csproj", "MicroservicesExample.Product/"]
RUN dotnet restore "MicroservicesExample.Product/MicroservicesExample.Product.csproj" 
COPY . .
WORKDIR "/src/MicroservicesExample.Product"
RUN dotnet build "MicroservicesExample.Product.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroservicesExample.Product.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroservicesExample.Product.dll"]