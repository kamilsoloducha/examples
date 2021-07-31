FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

##########################################################
# Build order service
##########################################################

WORKDIR /src
COPY ./src/StockService/StockService.csproj ./StockService/StockService.csproj
COPY ./src/Blueprints/Blueprints.csproj ./Blueprints/Blueprints.csproj
COPY ./src/Events/Events.csproj ./Events/Events.csproj

RUN dotnet restore ./StockService


COPY ./src/StockService ./StockService
COPY ./src/Blueprints ./Blueprints
COPY ./src/Events ./Events

RUN dotnet build ./StockService -o /stockservice

##########################################################
# Run order service
##########################################################

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS stockService

WORKDIR /stockservice
COPY --from=build /stockservice .
ENTRYPOINT dotnet StockService.dll