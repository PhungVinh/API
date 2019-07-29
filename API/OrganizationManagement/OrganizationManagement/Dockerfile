FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 52862
EXPOSE 44318

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY OrganizationManagement/OrganizationManagement.csproj OrganizationManagement/
RUN dotnet restore OrganizationManagement/OrganizationManagement.csproj
COPY . .
WORKDIR /src/OrganizationManagement
RUN dotnet build OrganizationManagement.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OrganizationManagement.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrganizationManagement.dll"]
