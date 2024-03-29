#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
WORKDIR /app
EXPOSE 80
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Receipts.ReadModel.API/Receipts.ReadModel.API.csproj", "Receipts.ReadModel.API/"]
COPY ["src/Receipts.ReadModel.CrossCutting/Receipts.ReadModel.CrossCutting.csproj", "Receipts.ReadModel.CrossCutting/"]
COPY ["src/Receipts.ReadModel.Application/Receipts.ReadModel.Application.csproj", "Receipts.ReadModel.Application/"]
COPY ["src/Receipts.ReadModel/Receipts.ReadModel.csproj", "Receipts.ReadModel/"]
COPY ["src/Receipts.ReadModel.Data/Receipts.ReadModel.Data.csproj", "Receipts.ReadModel.Data/"]
RUN dotnet restore "API/API.csproj"
COPY . .

RUN dotnet build "src/Receipts.ReadModel.API/Receipts.ReadModel.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Receipts.ReadModel.API/Receipts.ReadModel.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]