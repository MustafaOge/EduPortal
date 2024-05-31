#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EduPortal.MVC/EduPortal.MVC.csproj", "EduPortal.MVC/"]
COPY ["EduPortal.Application/EduPortal.Application.csproj", "EduPortal.Application/"]
COPY ["EduPortal.Domain/EduPortal.Domain.csproj", "EduPortal.Domain/"]
COPY ["EduPortal.Persistence/EduPortal.Persistence.csproj", "EduPortal.Persistence/"]
RUN dotnet restore "./EduPortal.MVC/./EduPortal.MVC.csproj"
COPY . .
WORKDIR "/src/EduPortal.MVC"
RUN dotnet build "./EduPortal.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EduPortal.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduPortal.MVC.dll"]