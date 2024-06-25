# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the official ASP.NET Core runtime image based on nanoserver 1809
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official ASP.NET Core SDK image based on nanoserver 1809 for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["KnowledgeSharingApi/KnowledgeSharingApi.csproj", "KnowledgeSharingApi/"]
COPY ["KnowledgeSharingApi.Domain/KnowledgeSharingApi.Domain.csproj", "KnowledgeSharingApi.Domain/"]
COPY ["KnowledgeSharingApi.Infrastructure/KnowledgeSharingApi.Infrastructure.csproj", "KnowledgeSharingApi.Infrastructure/"]
COPY ["KnowledgeSharingApi.Service/KnowledgeSharingApi.Service.csproj", "KnowledgeSharingApi.Service/"]
COPY ["KnowledgeSharingApi.Repositories/KnowledgeSharingApi.Repositories.csproj", "KnowledgeSharingApi.Repositories/"]
RUN dotnet restore "./KnowledgeSharingApi/KnowledgeSharingApi.csproj"

# Copy all source files and build the project
COPY . .
WORKDIR "/src/KnowledgeSharingApi"
RUN dotnet build "./KnowledgeSharingApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Create the publish stage using the build stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KnowledgeSharingApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage using the base runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "KnowledgeSharingApi.dll"]
