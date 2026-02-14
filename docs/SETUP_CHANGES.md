# Inicializačné zmeny - Oqtane Setup

## Súhrn vykonaných zmien

### 1. Program.cs konfigurácia ✅
- Vyčistenie a zjednodušenie `Program.cs`
- Správna konfigurácia Oqtane middleware s potrebnými službami z DI kontajnera
- Odstránenie zakomentovaného Blazor kódu

### 2. appsettings.json ✅
- ~~Pridanie `ConnectionStrings` pre SQLite databázu~~ **ZMENENÉ NA POSTGRESQL**
- Konfigurácia `ConnectionStrings.DefaultConnection` = "" (vyplní Aspire)
- Konfigurácia `Installation.InstallationMode` na `"Install"` pre prvotné spustenie
- Pridanie `Oqtane` sekcie s runtime nastaveniami
- **NOVÉ**: `Oqtane.DatabaseType` = `"PostgreSQL"`

### 3. appsettings.Development.json ✅
- ~~Pridanie connection string pre development~~ **ZMENENÉ - prázdny string**
- Rozšírené logovanie pre Entity Framework
- **NOVÉ**: Pridané `Npgsql` logovanie

### 4. ArtPortfolio.Web.csproj ✅
- Pridané build properties pre Oqtane:
  - `PreserveCompilationContext` = true
  - `MvcRazorCompileOnPublish` = false
- **NOVÉ**: Pridaný balíček `Npgsql.EntityFrameworkCore.PostgreSQL` v10.0.0

### 5. ArtPortfolio.AppHost/AppHost.cs ✅ **NOVÉ**
- Konfigurácia PostgreSQL databázy cez Aspire
- Parametrizované credentials (pg-username, pg-password)
- pgAdmin na porte 60751
- Automatické vytvorenie databázy `oqtane`
- `WaitFor(postgres)` dependency pre web aplikáciu

### 6. .gitignore ✅
- Pridané Oqtane-specific ignore pravidlá:
  - ~~Databázové súbory (`*.db`, `*.db-shm`, `*.db-wal`)~~ **Už nie je SQLite**
  - Runtime generované adresáre (`wwwroot/Modules/`, `wwwroot/Themes/`)
  - Tenant content (`Content/Tenants/*/`)
  - Upload adresáre

### 7. README.md ✅ **AKTUALIZOVANÉ**
- **NOVÉ**: PostgreSQL ako primárna databáza
- Kompletný návod na setup a spustenie s Aspire
- Inštrukcie pre Oqtane Installation Wizard s PostgreSQL
- Dokumentácia Aspire orchestration
- pgAdmin pripojenie
- Build a deployment príkazy
- Odkazy na novú dokumentáciu

### 8. Dockerfile ✅
- Multi-stage build pre optimalizáciu
- Inštalácia runtime dependencies (libgdiplus pre Oqtane)
- Vytvorenie potrebných adresárov
- Production-ready konfigurácia

### 9. .dockerignore ✅
- Optimalizácia Docker build kontextu
- Vylúčenie nepotrebných súborov

### 10. Dokumentácia ✅ **ROZŠÍRENÉ**
- ~~`docs/POSTGRESQL.md` - Návod na PostgreSQL konfiguráciu~~ **AKTUALIZOVANÉ**
- **NOVÉ**: `docs/AZURE_POSTGRESQL.md` - Kompletný Azure PostgreSQL guide
  - Cenové porovnanie Azure databáz
  - Free tier informácie ($0 prvý rok)
  - Azure CLI príkazy pre vytvorenie PostgreSQL Flexible Server
  - Connection strings pre lokálne aj Azure
  - Migrácia z SQLite na PostgreSQL
  - Monitoring a optimalizácie
- **NOVÉ**: `docs/QUICKSTART.md` - 5-minútový quick start guide
- `docs/MODULE_DEVELOPMENT.md` - Kompletný guide pre vývoj modulov

## Prvotné spustenie

```bash
# 1. Konfigurácia databázových credentials
cd ArtPortfolio.AppHost
dotnet user-secrets set "Parameters:pg-username" "postgres"
dotnet user-secrets set "Parameters:pg-password" "SecurePass123!"

# 2. Spustenie cez Aspire (automaticky spustí PostgreSQL)
cd ..
dotnet run --project ArtPortfolio.AppHost

# 3. Otvorte prehliadač na URL z Aspire dashboard
# 4. Prejdite Oqtane Installation Wizard:
#    - Database Type: PostgreSQL (automaticky vybraný)
#    - Connection string: (automaticky vyplnený z Aspire)
#    - Vytvorte admin účet
#    - Site Name: Art Portfolio
#    - Kliknite Install

# 5. Po úspešnej inštalácii, zmeňte v appsettings.json:
#    "InstallationMode": "None"
```

## Štruktúra po inštalácii

Po prvom spustení Oqtane vytvorí:

```
ArtPortfolio.Web/
├── Content/
│   └── Tenants/
│       └── Default/                 # Default tenant files
├── wwwroot/
│   ├── Modules/                     # Oqtane modules
│   └── Themes/                      # Oqtane themes
```

**Databáza**: PostgreSQL v Docker containeri (cez Aspire)
- Host: localhost
- Port: 5432
- Database: oqtane
- Username: postgres
- Password: (z user secrets)

## Prečo PostgreSQL?

### ✅ Výhody

1. **Najlacnejšia Azure databáza**:
   - Burstable B1ms: ~$12-15/mesiac
   - Free tier: 750 hodín/mesiac zdarma (prvý rok)

2. **Rovnaká databáza lokálne aj v Azure**:
   - Konzistentné prostredie
   - Žiadne problémy pri migrácii

3. **Oqtane plná podpora**:
   - Npgsql.EntityFrameworkCore.PostgreSQL v10.0.0
   - Všetky Oqtane funkcie fungujú

4. **Aspire natívna podpora**:
   - Automatická orchestrácia PostgreSQL containera
   - pgAdmin zadarmo
   - Zero configuration setup

5. **Enterprise ready**:
   - ACID compliance
   - Výborný performance
   - Azure managed service

### ⚠️ SQLite vs PostgreSQL

| Feature | SQLite | PostgreSQL |
|---------|--------|------------|
| Azure support | ❌ Nie | ✅ Áno |
| Multi-user | ⚠️ Obmedzené | ✅ Áno |
| Performance | ⚠️ Základný | ✅ Výborný |
| Backup/Recovery | ⚠️ Manuálne | ✅ Automatické |
| Cost | 🆓 Free | 💰 ~$12/mes (alebo free) |

## Verifikácia

- ✅ Build successful
- ✅ Všetky Oqtane NuGet packages nainštalované (10.0.3)
- ✅ Npgsql.EntityFrameworkCore.PostgreSQL v10.0.0
- ✅ ServiceDefaults integrácia funguje
- ✅ Aspire PostgreSQL orchestrácia pripravená
- ✅ Docker deployment pripravený
- ✅ Dokumentácia kompletná

## Nasledujúce kroky

1. **Prvotné spustenie**: 
   - Spustite `dotnet run --project ArtPortfolio.AppHost`
   - Prejdite Oqtane Installation Wizard
   
2. **Vytvorte admin účet**: Pre správu obsahu

3. **Nakonfigurujte site**: Nastavte názov, logo, theme

4. **Začnite vytvárať moduly**: Podľa `docs/MODULE_DEVELOPMENT.md`

5. **Azure deployment**: Podľa `docs/AZURE_POSTGRESQL.md`

## Poznámky

- **PostgreSQL** je primárna databáza (lokálne aj Azure)
- **SQLite** už nie je súčasťou projektu
- **Aspire** orchestrácia zjednodušuje lokálny development
- **Azure Container Apps** deployment je pripravený (Dockerfile ready)
- **Free tier Azure PostgreSQL** umožňuje nulové náklady prvý rok

## Užitočné príkazy

```bash
# Spustenie aplikácie
dotnet run --project ArtPortfolio.AppHost

# Build
dotnet build

# Prístup k pgAdmin
open http://localhost:60751

# Aspire dashboard
# (URL sa zobrazí v konzole po spustení)

# User secrets management
cd ArtPortfolio.AppHost
dotnet user-secrets list
dotnet user-secrets set "Parameters:pg-password" "NewPassword"
```

## Quick Links

- 🚀 [Quick Start Guide](QUICKSTART.md) - 5 minút do spustenia
- ☁️ [Azure PostgreSQL Guide](AZURE_POSTGRESQL.md) - Kompletný deployment guide
- 📘 [Module Development](MODULE_DEVELOPMENT.md) - Vývoj Oqtane modulov
- 🔧 [PostgreSQL Config](POSTGRESQL.md) - Databázová konfigurácia

---

**Dátum**: 2025-01-XX  
**Verzia**: 1.1.0-postgresql  
**Framework**: .NET 10 | Oqtane 10.0.3 | Aspire 13.0 | PostgreSQL 16
**Databáza**: PostgreSQL Flexible Server (lokálne + Azure)
