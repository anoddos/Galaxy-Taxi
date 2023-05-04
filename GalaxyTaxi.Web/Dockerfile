FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GalaxyTaxi.Web/GalaxyTaxi.Web.csproj", "GalaxyTaxi.Web/"]
RUN dotnet restore "GalaxyTaxi.Web/GalaxyTaxi.Web.csproj"
COPY . .
WORKDIR "/src/GalaxyTaxi.Web"
RUN dotnet build "GalaxyTaxi.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GalaxyTaxi.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaxyTaxi.Web.dll"]
