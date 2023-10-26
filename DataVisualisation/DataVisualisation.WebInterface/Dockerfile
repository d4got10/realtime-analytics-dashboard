FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["DataVisualisation.WebInterface/DataVisualisation.WebInterface.csproj", "DataVisualisation.WebInterface/"]
RUN dotnet restore "DataVisualisation.WebInterface/DataVisualisation.WebInterface.csproj"
COPY . .
WORKDIR "/src/DataVisualisation.WebInterface"
RUN dotnet build "DataVisualisation.WebInterface.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataVisualisation.WebInterface.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataVisualisation.WebInterface.dll"]
