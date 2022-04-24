#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Dockerfile

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["DauStore.Api/DauStore.Api.csproj", "DauStore.Api/"]
COPY ["DauStore.Core/DauStore.Core.csproj", "DauStore.Core/"]
COPY ["DauStore.Infrastructure/DauStore.Infrastructure.csproj", "DauStore.Infrastructure/"]
RUN dotnet restore "DauStore.Api/DauStore.Api.csproj"
COPY . .
WORKDIR "/src/DauStore.Api"
RUN dotnet build "DauStore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DauStore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DauStore.Api.dll"]