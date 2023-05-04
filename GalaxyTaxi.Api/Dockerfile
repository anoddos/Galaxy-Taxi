FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GalaxyTaxi/GalaxyTaxi.csproj", "GalaxyTaxi/"]
RUN dotnet restore "GalaxyTaxi/GalaxyTaxi.csproj"
COPY . .
WORKDIR "/src/GalaxyTaxi"
RUN dotnet build "GalaxyTaxi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GalaxyTaxi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaxyTaxi.dll"]
