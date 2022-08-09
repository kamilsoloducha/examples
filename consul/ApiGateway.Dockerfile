FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /server
COPY ./ApiGateway ./ApiGateway
COPY ./Blueprints ./Blueprints
RUN dotnet restore "./ApiGateway/ApiGateway.csproj"
RUN dotnet build "./ApiGateway/ApiGateway.csproj" -c Release -o /app

##########################################################
# Publish application
##########################################################

FROM build AS pre-release

RUN dotnet publish "./ApiGateway/ApiGateway.csproj" -c Release -o /app


##########################################################
# Run app
##########################################################

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS release

WORKDIR /app

COPY --from=pre-release /app .
ENV ASPNETCORE_ENVIRONMENT=Development
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ApiGateway.dll