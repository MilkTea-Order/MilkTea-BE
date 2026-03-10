FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 4000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY MilkTea.sln .
COPY ["MilkTea.API/MilkTea.API.csproj", "MilkTea.API/"]
COPY ["MilkTea.Application/MilkTea.Application.csproj", "MilkTea.Application/"]
COPY ["MilkTea.Domain/MilkTea.Domain.csproj", "MilkTea.Domain/"]
COPY ["MilkTea.Infrastructure/MilkTea.Infrastructure.csproj", "MilkTea.Infrastructure/"]
COPY ["MilkTea.Shared/MilkTea.Shared.csproj", "MilkTea.Shared/"]
RUN dotnet restore "MilkTea.API/MilkTea.API.csproj"
COPY . .
WORKDIR "/src/MilkTea.API"
RUN dotnet build "MilkTea.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MilkTea.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
LABEL maintainer="MilkTea Team"
LABEL description="Milk Tea Order System API"
LABEL version="1.0"
ENTRYPOINT ["dotnet", "MilkTea.API.dll"]