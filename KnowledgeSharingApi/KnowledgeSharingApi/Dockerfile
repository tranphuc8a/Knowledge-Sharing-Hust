# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the official ASP.NET Core image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Create a non-root user
# RUN addgroup --system app && adduser --system --ingroup app app

# Switch to non-root user
USER app

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["KnowledgeSharingApi/KnowledgeSharingApi.csproj", "KnowledgeSharingApi/"]
COPY ["KnowledgeSharingApi.Domain/KnowledgeSharingApi.Domains.csproj", "KnowledgeSharingApi.Domain/"]
COPY ["KnowledgeSharingApi.Infrastructure/KnowledgeSharingApi.Infrastructures.csproj", "KnowledgeSharingApi.Infrastructure/"]
COPY ["KnowledgeSharingApi.Service/KnowledgeSharingApi.Services.csproj", "KnowledgeSharingApi.Service/"]
COPY ["KnowledgeSharingApi.Repositories/KnowledgeSharingApi.Repositories.csproj", "KnowledgeSharingApi.Repositories/"]

# Restore dependencies
RUN dotnet restore "./KnowledgeSharingApi/KnowledgeSharingApi.csproj"

# Copy all files and build the project
COPY . .
WORKDIR "/src/KnowledgeSharingApi"
RUN dotnet build "./KnowledgeSharingApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KnowledgeSharingApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "KnowledgeSharingApi.dll"]
