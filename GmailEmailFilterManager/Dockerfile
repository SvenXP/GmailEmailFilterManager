# ── Stage 1: Build ─────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ── Stage 2: Runtime ───────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create App_Data folder for email persistence
RUN mkdir -p /app/App_Data

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GmailEmailFilterManager.dll"]
