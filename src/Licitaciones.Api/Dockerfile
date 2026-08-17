FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Licitaciones.sln ./
COPY Directory.Build.props ./
COPY global.json ./
COPY src/Licitaciones.Domain/Licitaciones.Domain.csproj src/Licitaciones.Domain/
COPY src/Licitaciones.Application/Licitaciones.Application.csproj src/Licitaciones.Application/
COPY src/Licitaciones.Infrastructure/Licitaciones.Infrastructure.csproj src/Licitaciones.Infrastructure/
COPY src/Licitaciones.Api/Licitaciones.Api.csproj src/Licitaciones.Api/
RUN dotnet restore src/Licitaciones.Api/Licitaciones.Api.csproj

COPY src/ src/
RUN dotnet publish src/Licitaciones.Api/Licitaciones.Api.csproj --configuration Release --no-restore --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "Licitaciones.Api.dll"]
