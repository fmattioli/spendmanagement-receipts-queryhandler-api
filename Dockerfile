#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
WORKDIR /app
EXPOSE 9563

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Receipts.QueryHandler.Api/Receipts.QueryHandler.Api.csproj", "Receipts.QueryHandler.Api/"]
COPY ["src/Receipts.QueryHandler.CrossCutting/Receipts.QueryHandler.CrossCutting.csproj", "Receipts.QueryHandler.CrossCutting/"]
COPY ["src/Receipts.QueryHandler.Application/Receipts.QueryHandler.Application.csproj", "Receipts.QueryHandler.Application/"]
COPY ["src/Receipts.QueryHandler.Domain/Receipts.QueryHandler.Domain.csproj", "Receipts.QueryHandler.Domain/"]
COPY ["src/Receipts.QueryHandler.Data/Receipts.QueryHandler.Data.csproj", "Receipts.QueryHandler.Data/"]
RUN dotnet restore "Receipts.QueryHandler.Api/Receipts.QueryHandler.Api.csproj"
COPY . .

RUN dotnet build "src/Receipts.QueryHandler.Api/Receipts.QueryHandler.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Receipts.QueryHandler.Api/Receipts.QueryHandler.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Receipts.QueryHandler.Api.dll"]