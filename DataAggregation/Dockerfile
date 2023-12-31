﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DataAggregation.API/DataAggregation.API.csproj", "DataAggregation.API/"]
COPY ["DataAggregation.Application/DataAggregation.Application.csproj", "DataAggregation.Application/"]
COPY ["DataAggregation.Infrastructure/DataAggregation.Infrastructure.csproj", "DataAggregation.Infrastructure/"]
COPY ["DataAggregation.Domain/DataAggregation.Domain.csproj", "DataAggregation.Domain/"]
RUN dotnet restore "DataAggregation.API/DataAggregation.API.csproj"
COPY . .
WORKDIR "/src/DataAggregation.API"
RUN dotnet build "DataAggregation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataAggregation.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY DataAggregation.API/appsettings.json /app/appsettings.json
ENTRYPOINT ["dotnet", "DataAggregation.API.dll"]
