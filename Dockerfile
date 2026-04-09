# ── Build Stage ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY StudentManagement/StudentManagement.csproj StudentManagement/
RUN dotnet restore StudentManagement/StudentManagement.csproj

COPY . .
WORKDIR /src/StudentManagement
RUN dotnet publish -c Release -o /app/publish

# ── Runtime Stage ───────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "StudentManagement.dll"]
