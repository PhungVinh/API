FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 51885
EXPOSE 44335

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY AttributesManagement/AttributesManagement.csproj AttributesManagement/
RUN dotnet restore AttributesManagement/AttributesManagement.csproj
COPY . .
WORKDIR /src/AttributesManagement
RUN dotnet build AttributesManagement.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AttributesManagement.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AttributesManagement.dll"]
