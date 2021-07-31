FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

##########################################################
# Build order service
##########################################################

WORKDIR /src
COPY ./src/ShippingService/ShippingService.csproj ./ShippingService/ShippingService.csproj
COPY ./src/Blueprints/Blueprints.csproj ./Blueprints/Blueprints.csproj

RUN dotnet restore ./ShippingService

COPY ./src/ShippingService ./ShippingService
COPY ./src/Blueprints ./Blueprints
RUN dotnet build ./ShippingService -o /shippingservice

##########################################################
# Run order service
##########################################################

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS shippingService

WORKDIR /shippingservice
COPY --from=build /shippingservice .
ENTRYPOINT dotnet ShippingService.dll