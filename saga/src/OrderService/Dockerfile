FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

##########################################################
# Build order service
##########################################################

WORKDIR /src
COPY ./src/OrderService/OrderService.csproj ./OrderService/OrderService.csproj
COPY ./src/Blueprints/Blueprints.csproj ./Blueprints/Blueprints.csproj
COPY ./src/Events/Events.csproj ./Events/Events.csproj

RUN dotnet restore ./OrderService

COPY ./src/OrderService ./OrderService
COPY ./src/Blueprints ./Blueprints
COPY ./src/Events ./Events
RUN dotnet build ./OrderService -o /orderservice

##########################################################
# Run order service
##########################################################

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS orderService

WORKDIR /orderservice
COPY --from=build /orderservice .
ENTRYPOINT dotnet OrderService.dll