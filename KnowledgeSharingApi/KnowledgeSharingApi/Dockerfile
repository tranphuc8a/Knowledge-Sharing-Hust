# See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, 
# the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# Base image for ASP.NET Core and .NET Framework support with IIS
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019 AS base
WORKDIR /inetpub/wwwroot
EXPOSE 80

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["KnowledgeSharingApi/KnowledgeSharingApi.csproj", "KnowledgeSharingApi/"]
COPY ["KnowledgeSharingApi.Domain/KnowledgeSharingApi.Domains.csproj", "KnowledgeSharingApi.Domain/"]
COPY ["KnowledgeSharingApi.Infrastructure/KnowledgeSharingApi.Infrastructures.csproj", "KnowledgeSharingApi.Infrastructure/"]
COPY ["KnowledgeSharingApi.Service/KnowledgeSharingApi.Services.csproj", "KnowledgeSharingApi.Service/"]
COPY ["KnowledgeSharingApi.Repositories/KnowledgeSharingApi.Repositories.csproj", "KnowledgeSharingApi.Repositories/"]
RUN nuget restore "KnowledgeSharingApi/KnowledgeSharingApi.csproj"
RUN dotnet restore "KnowledgeSharingApi/KnowledgeSharingApi.csproj"

# Copy all files and build the project
COPY . .
WORKDIR "/src/KnowledgeSharingApi"
RUN msbuild /p:Configuration=%BUILD_CONFIGURATION%

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "KnowledgeSharingApi/KnowledgeSharingApi.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /inetpub/wwwroot
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KnowledgeSharingApi.dll"]
