FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY listing.sln ./
COPY listing.data/*.csproj ./listing.data/
COPY listing.api/*.csproj ./listing.api/
COPY listing.unit-tests/*.csproj ./listing.unit-tests/

RUN dotnet restore
COPY . .
WORKDIR /src/listing.data
RUN dotnet build -c Release -o /app

WORKDIR /src/listing.api
RUN dotnet build -c Release -o /app

WORKDIR /src/listing.unit-tests
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "listing.api.dll"]