# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/catalog/catalog.API/catalog.API.csproj", "services/catalog/catalog.API/"]
COPY ["services/catalog/catalog.Application/catalog.Application.csproj", "services/catalog/catalog.Application/"]
COPY ["services/catalog/catalog.Core/catalog.Core.csproj", "services/catalog/catalog.Core/"]
COPY ["services/catalog/catalog.Infrastructure/catalog.Infrastructure.csproj", "services/catalog/catalog.Infrastructure/"]
COPY ["Infrastructure/Common.Logging/Common.Logging.csproj","Infrastructure/Common.Logging/"]
RUN dotnet restore "./services/catalog/catalog.API/catalog.API.csproj"
COPY . .
WORKDIR "/src/services/catalog/catalog.API"
RUN dotnet build "./catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "catalog.API.dll"]