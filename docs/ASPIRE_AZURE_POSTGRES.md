# Aspire Azure PostgreSQL Integration Guide

Návod na použitie `Aspire.Hosting.Azure.PostgreSQL` pre Azure Database for PostgreSQL Flexible Server.

## 📦 NuGet balíček

**Package**: `Aspire.Hosting.Azure.PostgreSQL`  
**Version**: 13.1.0  
**NuGet**: https://www.nuget.org/packages/Aspire.Hosting.Azure.PostgreSQL

## 🎯 Čo to robí?

Aspire Azure PostgreSQL hosting umožňuje:
1. ✅ **Lokálny vývoj**: Docker PostgreSQL container (cez `AddPostgres()`)
2. ✅ **Azure deployment**: Automatické prepojenie na Azure PostgreSQL Flexible Server
3. ✅ **Zero-config switch**: Jedna konfigurácia, prepínanie cez environment variable

---

## 🚀 Setup v ArtPortfolio.AppHost

### 1. Už je nainštalované! ✅

```xml
<PackageReference Include="Aspire.Hosting.Azure.PostgreSQL" Version="13.1.0" />
```

### 2. AppHost.cs konfigurácia

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Choose deployment mode
var useAzure = builder.Configuration["UseAzurePostgreSQL"] == "true";

if (useAzure)
{
    // Azure PostgreSQL Flexible Server
    var postgres = builder.AddAzurePostgresFlexibleServer("postgres")
        .AddDatabase("oqtane");

    builder.AddProject<Projects.ArtPortfolio_Web>("artportfolio-web")
        .WithReference(postgres);
}
else
{
    // Local Docker PostgreSQL
    var pgUsername = builder.AddParameter("pg-username");
    var pgPassword = builder.AddParameter("pg-password", secret: true);

    var postgres = builder.AddPostgres("postgres", pgUsername, pgPassword)
        .WithDataVolume()
        .WithPgAdmin();

    var oqtaneDb = postgres.AddDatabase("oqtane");

    builder.AddProject<Projects.ArtPortfolio_Web>("artportfolio-web")
        .WithReference(oqtaneDb)
        .WaitFor(postgres);
}

await builder.Build().RunAsync();
```

---

## 🏠 Lokálny vývoj (Default)

### Spustenie bez Azure

```bash
# User secrets pre lokálnu databázu
cd ArtPortfolio.AppHost
dotnet user-secrets set "Parameters:pg-username" "postgres"
dotnet user-secrets set "Parameters:pg-password" "YourPassword123!"

# Spustenie
dotnet run --project ArtPortfolio.AppHost
```

**Čo sa stane:**
- Docker PostgreSQL container sa spustí
- pgAdmin na http://localhost:60751
- Connection string sa automaticky nastaví

---

## ☁️ Azure PostgreSQL deployment

### Krok 1: Vytvorenie Azure PostgreSQL

**Cez Azure Portal**:
👉 https://portal.azure.com/#create/Microsoft.PostgreSQLServer

**Alebo Azure CLI**:

```bash
# Login
az login

# Vytvorenie resource group
az group create --name rg-artportfolio --location westeurope

# Vytvorenie PostgreSQL Flexible Server
az postgres flexible-server create \
  --resource-group rg-artportfolio \
  --name artportfolio-pg \
  --location westeurope \
  --admin-user pgadmin \
  --admin-password "SecurePassword123!" \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --version 16 \
  --storage-size 32 \
  --public-access 0.0.0.0

# Vytvorenie databázy
az postgres flexible-server db create \
  --resource-group rg-artportfolio \
  --server-name artportfolio-pg \
  --database-name oqtane
```

### Krok 2: Konfigurácia Aspire pre Azure

**Metóda 1: Environment variable (Odporúčané)**

```bash
# Nastavenie environment variable
export UseAzurePostgreSQL=true

# Alebo v PowerShell
$env:UseAzurePostgreSQL="true"

# Spustenie
dotnet run --project ArtPortfolio.AppHost
```

**Metóda 2: appsettings.json**

```json
{
  "UseAzurePostgreSQL": "true"
}
```

### Krok 3: Azure connection string

Aspire potrebuje vedieť, ako sa pripojiť na Azure PostgreSQL:

**Option A: azd deploy (Automatické)**

```bash
# Azure Developer CLI automaticky detekuje Aspire resources
azd init
azd up
```

**Option B: Manuálna konfigurácia**

```bash
# Connection string do environment variable
export AZURE_POSTGRES_CONNECTION_STRING="Host=artportfolio-pg.postgres.database.azure.com;Database=oqtane;Username=pgadmin;Password=SecurePassword123!;SSL Mode=Require"
```

---

## 🔐 Entra ID (Azure AD) Authentication

Azure PostgreSQL Flexible Server v Aspire **defaultne používa Entra ID** (Azure Active Directory).

### Výhody:
- ✅ **Bez hesiel** (passwordless authentication)
- ✅ **Managed Identity** pre Container Apps
- ✅ **Bezpečnejšie** než SQL authentication

### Ako to funguje:

```csharp
// Aspire automaticky konfiguruje Entra ID authentication
var postgres = builder.AddAzurePostgresFlexibleServer("postgres")
    .AddDatabase("oqtane");
```

**V aplikácii (ArtPortfolio.Web)**:

Npgsql automaticky použije Azure credentials:

```csharp
// Npgsql automaticky získa token cez DefaultAzureCredential
// Žiadny extra kód nie je potrebný!
```

### Ak chcete použiť password authentication:

```csharp
var keyVault = builder.AddAzureKeyVault("vault");

var username = builder.AddParameter("pg-username");
var password = builder.AddParameter("pg-password", secret: true);

var postgres = builder.AddAzurePostgresFlexibleServer("postgres")
    .WithPasswordAuthentication(keyVault, username, password)
    .AddDatabase("oqtane");
```

---

## 🔄 Prepínanie medzi lokálnym a Azure

### Vývojový workflow:

```bash
# 1. Lokálny vývoj (default)
dotnet run --project ArtPortfolio.AppHost

# 2. Test s Azure
export UseAzurePostgreSQL=true
dotnet run --project ArtPortfolio.AppHost

# 3. Späť na lokálne
unset UseAzurePostgreSQL
dotnet run --project ArtPortfolio.AppHost
```

### launchSettings.json konfigurácia

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https-azure": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "UseAzurePostgreSQL": "true"
      }
    }
  }
}
```

Potom v Visual Studio vyberte profil `https-azure` pre testovanie s Azure.

---

## 📝 Manifest (azd)

Pri použití `azd deploy`, Aspire generuje manifest pre Azure:

```bash
# Vygenerovanie manifestu
dotnet run --project ArtPortfolio.AppHost --publisher manifest --output-path manifest.json

# Deploy cez azd
azd init
azd up
```

**azd automaticky:**
1. Vytvorí Azure PostgreSQL Flexible Server
2. Nakonfiguruje Managed Identity
3. Nastaví firewall rules
4. Pripojí Container Apps k databáze

---

## 🔍 Debugging

### Zistenie, ktorá databáza sa používa

```bash
# Skontrolovať environment variable
echo $UseAzurePostgreSQL

# Skontrolovať Aspire dashboard
# Po spustení otvorte Aspire dashboard URL (zobrazí sa v konzole)
# V dashboarde uvidíte, či je to Docker alebo Azure resource
```

### Connection string verifikácia

V Aspire dashboarde:
1. Otvorte resource `artportfolio-web`
2. Choďte na **Environment variables**
3. Hľadajte `ConnectionStrings__oqtane`

---

## 🎯 Production deployment

### Azure Container Apps + Azure PostgreSQL

```bash
# 1. Login
az login
azd auth login

# 2. Inicializácia
azd init

# 3. Deploy (vytvorí všetko)
azd up
```

**azd vytvorí:**
- Azure PostgreSQL Flexible Server (B1ms tier)
- Azure Container Apps Environment
- Container Apps pre web aplikáciu
- Managed Identity pre passwordless auth
- Všetky potrebné network settings

---

## 💰 Náklady

| Resource | Tier | Cena/mes |
|----------|------|----------|
| PostgreSQL Flexible | B1ms | ~$12-15 |
| PostgreSQL Flexible | Free (750h) | $0 |
| Container Apps | Free tier | $0 |
| Container Registry | Basic | ~$5 |
| **Total** | | **~$17-20** (alebo $5 s free tier) |

---

## 📚 Ďalšie zdroje

- **Aspire PostgreSQL docs**: https://learn.microsoft.com/dotnet/aspire/database/postgresql-component
- **Azure PostgreSQL Flexible**: https://learn.microsoft.com/azure/postgresql/flexible-server/
- **Aspire Azure deployment**: https://learn.microsoft.com/dotnet/aspire/deployment/azure/overview
- **azd CLI**: https://learn.microsoft.com/azure/developer/azure-developer-cli/

---

## ✅ Checklist

### Lokálny vývoj
- [x] `Aspire.Hosting.Azure.PostgreSQL` nainštalovaný
- [x] User secrets nastavené
- [x] Docker Desktop spustený
- [x] `dotnet run --project ArtPortfolio.AppHost`

### Azure deployment
- [ ] Azure subscription aktívna
- [ ] `azd` CLI nainštalovaný
- [ ] `azd auth login`
- [ ] `azd init`
- [ ] `azd up`

---

**Aktualizované**: 2025-01-XX  
**Verzia**: 1.0  
**Aspire**: 13.1.0
