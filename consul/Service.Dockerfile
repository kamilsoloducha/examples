FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /server
COPY ./Service ./Service
COPY ./Blueprints ./Blueprints
RUN dotnet restore "./Service/Service.csproj"
RUN dotnet build "./Service/Service.csproj" -c Release -o /app

##########################################################
# Publish application
##########################################################

FROM build AS pre-release

RUN dotnet publish "./Service/Service.csproj" -c Release -o /app


##########################################################
# Run app
##########################################################

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS release

WORKDIR /app

COPY --from=pre-release /app .
ENV ASPNETCORE_ENVIRONMENT=Development
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Service.dll