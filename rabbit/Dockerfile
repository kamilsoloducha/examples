FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ./src .
RUN dotnet build ./Service1 -o /service1
RUN dotnet build ./Service2 -o /service2
RUN dotnet build ./Service3 -o /service3

##########################################################
# Run service1
##########################################################

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS service1

WORKDIR /service1
COPY --from=build /service1 .
ENTRYPOINT dotnet Service1.dll

##########################################################
# Run service2
##########################################################

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS service2

WORKDIR /service2
COPY --from=build /service2 .
ENTRYPOINT dotnet Service2.dll

##########################################################
# Run service3
##########################################################

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS service3

WORKDIR /app
COPY --from=build /service3 .
ENTRYPOINT dotnet Service3.dll