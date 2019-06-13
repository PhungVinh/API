FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 51316
EXPOSE 44341

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ../Share/Share.csproj ../Share/
RUN dotnet restore ../Share/Share.csproj
COPY . .
WORKDIR /src/../Share
RUN dotnet build Share.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Share.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Share.dll"]
