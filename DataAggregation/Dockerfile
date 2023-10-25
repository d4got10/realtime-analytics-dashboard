FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DataAggregation.CLI/DataAggregation.CLI.csproj", "DataAggregation.CLI/"]
COPY ["DataAggregation.Application/DataAggregation.Application.csproj", "DataAggregation.Application/"]
COPY ["DataAggregation.Infrastructure/DataAggregation.Infrastructure.csproj", "DataAggregation.Infrastructure/"]
RUN dotnet restore "DataAggregation.CLI/DataAggregation.CLI.csproj"
COPY . .
WORKDIR "/src/DataAggregation.CLI"
RUN dotnet build "DataAggregation.CLI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataAggregation.CLI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY DataAggregation.CLI/appsettings.json /app/appsettings.json
ENTRYPOINT ["dotnet", "DataAggregation.CLI.dll"]
