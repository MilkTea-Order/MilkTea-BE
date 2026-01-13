# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER $APP_UID
WORKDIR /app
EXPOSE 4000

# EXPOSE 8080
# EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MilkTea.API/MilkTea.API.csproj", "MilkTea.API/"]
RUN dotnet restore "./MilkTea.API/MilkTea.API.csproj"
COPY . .
WORKDIR "/src/MilkTea.API"
RUN dotnet build "./MilkTea.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MilkTea.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add labels for metadata
LABEL maintainer="MilkTea Team"
LABEL description="Milk Tea Order System API"
LABEL version="1.0"

ENTRYPOINT ["dotnet", "MilkTea.API.dll"]