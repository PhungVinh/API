FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 56172
EXPOSE 44341

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ../KMS/KMS.csproj ../KMS/
RUN dotnet restore ../KMS/KMS.csproj
COPY . .
WORKDIR /src/../KMS
RUN dotnet build KMS.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish KMS.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KMS.dll"]
