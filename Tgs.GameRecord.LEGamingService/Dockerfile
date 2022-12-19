#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.LEGamingService/Tgs.GameRecord.LEGamingService.csproj", "Tgs.GameRecord.LEGamingService/"]
COPY ["Tgs.GameRecord.LEGamingService.Core/Tgs.GameRecord.LEGamingService.Core.csproj", "Tgs.GameRecord.LEGamingService.Core/"]
RUN dotnet restore "Tgs.GameRecord.LEGamingService/Tgs.GameRecord.LEGamingService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.LEGamingService"
RUN dotnet build "Tgs.GameRecord.LEGamingService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameRecord.LEGamingService.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.LEGamingService.dll"]