FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Authorization/Authorization.csproj", "Authorization/"]
RUN dotnet restore "Authorization/Authorization.csproj"
COPY . .
WORKDIR "/src/Authorization"
RUN dotnet build "Authorization.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authorization.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authorization.dll"]
