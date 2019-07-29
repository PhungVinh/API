FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 56040
EXPOSE 44371

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ../VOC/VOC.csproj ../VOC/
RUN dotnet restore ../VOC/VOC.csproj
COPY . .
WORKDIR /src/../VOC
RUN dotnet build VOC.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish VOC.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VOC.dll"]
