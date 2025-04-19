FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TABP.WebAPI/TABP.WebAPI.csproj", "TABP.WebAPI/"]
COPY ["TABP.Application/TABP.Application.csproj", "TABP.Application/"]
COPY ["TABP.Domain/TABP.Domain.csproj", "TABP.Domain/"]
COPY ["TABP.Infrastructure/TABP.Infrastructure.csproj", "TABP.Infrastructure/"]
COPY ["TABP.Presentation/TABP.Presentation.csproj", "TABP.Presentation/"]
RUN dotnet restore "./TABP.WebAPI/TABP.WebAPI.csproj"
COPY . .
WORKDIR "/src/TABP.WebAPI"
RUN dotnet build "./TABP.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TABP.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TABP.WebAPI.dll"]