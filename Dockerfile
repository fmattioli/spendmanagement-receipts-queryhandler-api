#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y curl
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/API/API.csproj", "API/"]
COPY ["src/CrossCutting/CrossCutting.csproj", "CrossCutting/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Web.Contracts/Web.Contracts.csproj", "Web.Contracts/"]
COPY ["src/Data/Data.csproj", "Data/"]
RUN dotnet restore "API/API.csproj"
COPY . .

RUN dotnet build "src/API/API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/API/API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]