FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DataCollection.API/DataCollection.API.csproj", "DataCollection.API/"]
COPY ["DataCollection.Application/DataCollection.Application.csproj", "DataCollection.Application/"]
COPY ["DataCollection.Infrastructure/DataCollection.Infrastructure.csproj", "DataCollection.Infrastructure/"]
RUN dotnet restore "DataCollection.API/DataCollection.API.csproj"
COPY . .
WORKDIR "/src/DataCollection.API"
RUN dotnet build "DataCollection.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataCollection.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataCollection.API.dll"]
