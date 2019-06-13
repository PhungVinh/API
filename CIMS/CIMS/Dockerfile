FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 52821
EXPOSE 44315

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY CIMS/CIMS.csproj CIMS/
RUN dotnet restore CIMS/CIMS.csproj
COPY . .
WORKDIR /src/CIMS
RUN dotnet build CIMS.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CIMS.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CIMS.dll"]
