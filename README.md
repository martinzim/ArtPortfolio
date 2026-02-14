# ArtPortfolio

Webové portfólio pre digitálneho ilustrátora postavené na **Oqtane CMS** a **.NET 10 Blazor Server**.

## 🚀 Technológie

- **.NET 10** (C# 14.0)
- **Blazor Server** (interactive server-side rendering)
- **Oqtane CMS 10.0.3** (headless/integrated CMS)
- **.NET Aspire 13.0** (local development orchestration)
- **PostgreSQL** (lokálne + Azure Database for PostgreSQL Flexible Server)
- **Azure Container Apps** (production deployment target)

## 📁 Štruktúra projektu

```
ArtPortfolio/
├── ArtPortfolio.Web/           # Hlavná Blazor Server aplikácia s Oqtane
├── ArtPortfolio.AppHost/        # Aspire orchestration host
├── ArtPortfolio.ServiceDefaults/ # Zdieľané predvoľby služieb (telemetria, kontrola stavu)
└── docs/                        # Dokumentácia
    ├── AZURE_POSTGRESQL.md      # Sprievodca nastavením Azure databázy
    ├── POSTGRESQL.md            # Konfigurácia PostgreSQL
    └── MODULE_DEVELOPMENT.md    # Sprievodca vývojom Oqtane modulov
```

## 🛠️ Prvotné nastavenie

### Požiadavky

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (pre PostgreSQL container)
- [Visual Studio 2025](https://visualstudio.microsoft.com/) alebo [Visual Studio Code](https://code.visualstudio.com/)
- [.NET Aspire Workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling)

### Inštalácia .NET Aspire

```bash
dotnet workload update
dotnet workload install aspire
```

### Konfigurácia databázy (PostgreSQL)

```bash
cd ArtPortfolio.AppHost
dotnet user-secrets init
dotnet user-secrets set "Parameters:pg-username" "postgres"
dotnet user-secrets set "Parameters:pg-password" "YourSecurePassword123!"
```

## 🚀 Spustenie aplikácie

### Lokálny vývoj (s Aspire + PostgreSQL)

```bash
# Spustenie cez Aspire orchestráciu
dotnet run --project ArtPortfolio.AppHost
```

Aspire automaticky:
- Spustí PostgreSQL Docker container
- Vytvorí databázu `oqtane`
- Spustí pgAdmin na http://localhost:60751
- Spustí web aplikáciu
- Otvorí Aspire dashboard

### Priame spustenie (bez Aspire)

```bash
cd ArtPortfolio.Web
dotnet run
```

⚠️ **Poznámka**: Budete potrebovať manuálne nastaviť PostgreSQL connection string v `appsettings.Development.json`

## 📝 Prvotná inštalácia Oqtane

Pri prvom spustení sa automaticky spustí **Oqtane Installation Wizard**:

1. Otvorte aplikáciu v prehliadači
2. Vyberte **Database Type**: **PostgreSQL**
3. Connection string bude už predvyplnený (z Aspire)
4. Vytvorte **Admin účet**:
   - Username
   - Password
   - Email
5. Zadajte **Site Name**: napr. "Art Portfolio"
6. Kliknite na **Install**

Po inštalácii sa vytvorí:
- PostgreSQL databázová schéma
- `Content/Tenants/Default/` - súbory pre default tenant
- `wwwroot/Modules/` - Oqtane moduly
- `wwwroot/Themes/` - Oqtane témy

⚠️ **Dôležité**: Po úspešnej inštalácii zmeňte v `appsettings.json`:
```json
"Installation": {
  "InstallationMode": "None"
}
```

## 🔧 Konfigurácia

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Installation": {
    "InstallationMode": "Install"
  },
  "Oqtane": {
    "InstallationFiles": "wwwroot",
    "Runtime": "Server",
    "SiteTemplate": "Default Site Template",
    "DatabaseType": "PostgreSQL"
  }
}
```

**Poznámka**: Connection string sa automaticky nastaví cez Aspire pri lokálnom vývoji.

## 💾 Databáza

### Lokálny vývoj
- **PostgreSQL 16** (Docker container cez Aspire)
- **pgAdmin** dostupný na http://localhost:60751

### Azure Production
- **Azure Database for PostgreSQL Flexible Server**
- **Burstable B1ms tier**: ~$12/mesiac
- **Free tier**: 750 hodín/mesiac zdarma (prvý rok)

📚 **Detailné guides**:
- [docs/AZURE_POSTGRESQL.md](docs/AZURE_POSTGRESQL.md) - Azure deployment
- [docs/DATABASE_COMPARISON.md](docs/DATABASE_COMPARISON.md) - Porovnanie Azure databáz

## 🏗️ Vývoj

### Build

```bash
dotnet build
```

### Test

```bash
dotnet test
```

### Vytvorenie nového Oqtane modulu

Pozri dokumentáciu v [docs/MODULE_DEVELOPMENT.md](docs/MODULE_DEVELOPMENT.md) pre detailný návod.

## 📦 Deployment

### Azure Container Apps

```bash
# Build container image
docker build -t artportfolio-web:latest -f ArtPortfolio.Web/Dockerfile .

# Push to Azure Container Registry
az acr login --name yourregistry
docker tag artportfolio-web:latest yourregistry.azurecr.io/artportfolio-web:latest
docker push yourregistry.azurecr.io/artportfolio-web:latest
```

📚 **Azure deployment guide**: [docs/AZURE_POSTGRESQL.md](docs/AZURE_POSTGRESQL.md)

## 📚 Dokumentácia

- [docs/ASPIRE_AZURE_POSTGRES.md](docs/ASPIRE_AZURE_POSTGRES.md) - **NOVÉ!** Aspire Azure PostgreSQL integration
- [docs/QUICKSTART.md](docs/QUICKSTART.md) - 5-minútový quick start
- [docs/AZURE_POSTGRESQL.md](docs/AZURE_POSTGRESQL.md) - Azure database setup a deployment
- [docs/DATABASE_COMPARISON.md](docs/DATABASE_COMPARISON.md) - Porovnanie Azure databáz
- [docs/POSTGRESQL.md](docs/POSTGRESQL.md) - PostgreSQL konfigurácia
- [docs/MODULE_DEVELOPMENT.md](docs/MODULE_DEVELOPMENT.md) - Vývoj Oqtane modulov
- [docs/ENVIRONMENT_CONFIG.md](docs/ENVIRONMENT_CONFIG.md) - Environment variables
- [docs/SETUP_CHANGES.md](docs/SETUP_CHANGES.md) - História zmien
- [.github/copilot-instructions.md](.github/copilot-instructions.md) - Development guidelines

### Externé zdroje

- [Oqtane Documentation](https://docs.oqtane.org/)
- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [Blazor Documentation](https://learn.microsoft.com/aspnet/core/blazor/)

## 🔍 pgAdmin pripojenie

Po spustení Aspire:

- **URL**: http://localhost:60751
- **Email**: admin@admin.com (default)
- **Password**: admin (default)

Pridať server:
- **Host**: postgres (container name)
- **Port**: 5432
- **Database**: oqtane
- **Username**: postgres
- **Password**: (vaše heslo z user secrets)

## 📄 Licencia

Proprietárne - všetky práva vyhradené.

---

**Version**: 1.0.0  
**Created**: 2025  
**Framework**: .NET 10 | Oqtane 10.0.3 | PostgreSQL 16
