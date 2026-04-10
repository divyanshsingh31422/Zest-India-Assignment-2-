# Use the official .NET 8.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["StudentManagement.Api/StudentManagement.Api.csproj", "StudentManagement.Api/"]
COPY ["StudentManagement.Tests/StudentManagement.Tests.csproj", "StudentManagement.Tests/"]
RUN dotnet restore "StudentManagement.Api/StudentManagement.Api.csproj"

# Copy the rest of the application
COPY . .

# Build the application
WORKDIR "/src/StudentManagement.Api"
RUN dotnet build "StudentManagement.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "StudentManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "StudentManagement.Api.dll"]
