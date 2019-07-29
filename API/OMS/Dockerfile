FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 56054
EXPOSE 44316

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ../OMS/OMS.csproj ../OMS/
RUN dotnet restore ../OMS/OMS.csproj
COPY . .
WORKDIR /src/../OMS
RUN dotnet build OMS.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OMS.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OMS.dll"]
