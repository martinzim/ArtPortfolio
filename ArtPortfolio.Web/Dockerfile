# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["ArtPortfolio.Web/ArtPortfolio.Web.csproj", "ArtPortfolio.Web/"]
COPY ["ArtPortfolio.ServiceDefaults/ArtPortfolio.ServiceDefaults.csproj", "ArtPortfolio.ServiceDefaults/"]
RUN dotnet restore "ArtPortfolio.Web/ArtPortfolio.Web.csproj"

# Copy remaining source code
COPY . .

# Build the application
WORKDIR "/src/ArtPortfolio.Web"
RUN dotnet build "ArtPortfolio.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ArtPortfolio.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install required runtime dependencies for Oqtane
RUN apt-get update && apt-get install -y \
    libgdiplus \
    libc6-dev \
    && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Create directories for Oqtane runtime
RUN mkdir -p /app/Content/Tenants/Default \
    && mkdir -p /app/wwwroot/Modules \
    && mkdir -p /app/wwwroot/Themes

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 8080

ENTRYPOINT ["dotnet", "ArtPortfolio.Web.dll"]
