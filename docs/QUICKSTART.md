# Quick Start Guide - ArtPortfolio

Tento návod vás prevedie prvotným spustením aplikácie s PostgreSQL databázou.

## ⚡ Rýchly štart (5 minút)

### 1. Požiadavky

Uistite sa, že máte nainštalované:
- ✅ [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- ✅ [Docker Desktop](https://www.docker.com/products/docker-desktop) (musí byť spustený!)
- ✅ [Visual Studio 2025](https://visualstudio.microsoft.com/) alebo [VS Code](https://code.visualstudio.com/)

### 2. Inštalácia .NET Aspire

```bash
dotnet workload update
dotnet workload install aspire
```

### 3. Konfigurácia databázových credentials

```bash
cd ArtPortfolio.AppHost
dotnet user-secrets init
dotnet user-secrets set "Parameters:pg-username" "postgres"
dotnet user-secrets set "Parameters:pg-password" "SecurePass123!"
```

💡 **Tip**: Heslo si zapamätajte - budete ho potrebovať pre pgAdmin.

### 4. Spustenie aplikácie

```bash
# Z koreňového adresára projektu
dotnet run --project ArtPortfolio.AppHost
```

**Čo sa stane:**
1. ✅ Aspire stiahne a spustí PostgreSQL Docker container
2. ✅ Vytvorí databázu `oqtane`
3. ✅ Spustí pgAdmin na http://localhost:60751
4. ✅ Spustí ArtPortfolio web aplikáciu
5. ✅ Otvorí Aspire Dashboard

### 5. Oqtane Installation Wizard

Po spustení sa automaticky otvorí **Oqtane Installation Wizard**:

1. **Database Configuration**:
   - Database Type: **PostgreSQL** (už by mal byť vybraný)
   - Connection String: (už predvyplnený z Aspire)
   
2. **Administrator Account**:
   - Username: `admin`
   - Password: (silné heslo)
   - Email: `your@email.com`

3. **Site Configuration**:
   - Site Name: `Art Portfolio`
   - Alias: `localhost`

4. Kliknite **Install** a počkajte ~30 sekúnd

5. **Po úspešnej inštalácii**:
   - Prihláste sa s admin účtom
   - V `appsettings.json` zmeňte:
     ```json
     "Installation": {
       "InstallationMode": "None"
     }
     ```

### 6. Overenie

**Web aplikácia**: http://localhost:5124 (alebo URL z Aspire dashboard)
**Aspire Dashboard**: http://localhost:15000 (alebo podobný port)
**pgAdmin**: http://localhost:60751

## 🔍 pgAdmin pripojenie

Ak chcete priamo pristupovať k databáze:

1. Otvorte http://localhost:60751
2. Prihlásenie:
   - Email: `admin@admin.com`
   - Password: `admin`
3. Pridať server:
   - Name: `Oqtane Local`
   - Host: `postgres` (názov Aspire containera)
   - Port: `5432`
   - Database: `oqtane`
   - Username: `postgres`
   - Password: (vaše heslo z user secrets)

## ❗ Riešenie problémov

### Docker Desktop nie je spustený
```
Error: Cannot connect to Docker daemon
```
**Riešenie**: Spustite Docker Desktop a počkajte, kým sa úplne načíta.

### Port už používaný
```
Error: Address already in use
```
**Riešenie**: Ukončite aplikáciu, ktorá používa port 5432 alebo 60751, alebo zmeňte port v `AppHost.cs`.

### Connection string nie je nastavený
```
Error: No connection string found
```
**Riešenie**: Skontrolujte, či ste nastavili user secrets (krok 3).

### Aspire Dashboard sa neotvorí
**Riešenie**: 
- Skontrolujte output v konzole pre správny URL
- Manuálne otvorte URL uvedený v konzole

## 🎯 Ďalšie kroky

Po úspešnom spustení:

1. ✅ Prihláste sa do Oqtane admin panelu
2. ✅ Preskúmajte default site template
3. ✅ Začnite vytvárať obsah
4. ✅ Naučte sa vytvárať vlastné moduly: [docs/MODULE_DEVELOPMENT.md](MODULE_DEVELOPMENT.md)

## 📚 Komplexná dokumentácia

- [README.md](../README.md) - Hlavná dokumentácia
- [AZURE_POSTGRESQL.md](AZURE_POSTGRESQL.md) - Azure deployment guide
- [MODULE_DEVELOPMENT.md](MODULE_DEVELOPMENT.md) - Vývoj Oqtane modulov

---

**Potrebujete pomoc?** Otvorte issue na GitHub alebo kontaktujte vývojára.
