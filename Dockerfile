# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image for compiling the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies first to leverage Docker cache
COPY ["Redarbor.sln", "."]
COPY ["src/Redarbor.Api/Redarbor.Api.csproj", "src/Redarbor.Api/"]
COPY ["src/Redarbor.Application/Redarbor.Application.csproj", "src/Redarbor.Application/"]
COPY ["src/Redarbor.Core/Redarbor.Core.csproj", "src/Redarbor.Core/"]
COPY ["src/Redarbor.Infrastructure/Redarbor.Infrastructure.csproj", "src/Redarbor.Infrastructure/"]
COPY ["tests/Redarbor.Tests/Redarbor.Tests.csproj", "tests/Redarbor.Tests/"]
RUN dotnet restore "Redarbor.sln"

# Copy the rest of the source code
COPY . .

# Build the API project
WORKDIR "/src/src/Redarbor.Api"
RUN dotnet build "Redarbor.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Redarbor.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Redarbor.Api.dll"]
