# Azure Database Comparison for Oqtane CMS

Porovnanie Azure databázových služieb vhodných pre Oqtane CMS aplikáciu.

## 🎯 TL;DR - Odporúčanie

**Pre ArtPortfolio**: **Azure Database for PostgreSQL Flexible Server (Burstable B1ms)**

- 💰 Cena: ~$12-15/mesiac (alebo FREE tier prvý rok)
- ✅ Oqtane plná podpora
- ✅ Rovnaká databáza lokálne aj v Azure
- ✅ .NET Aspire natívna podpora

---

## 📊 Detailné porovnanie

### 1. Azure Database for PostgreSQL Flexible Server ⭐ ODPORÚČAM

| Kritérium | Hodnota |
|-----------|---------|
| **Cena (min)** | ~$12/mes (B1ms) alebo **FREE** 750h/mes |
| **Výkon (min)** | 1 vCore, 2 GB RAM |
| **Storage (min)** | 32 GB |
| **Oqtane podpora** | ✅ Výborná (Npgsql v10.0.0) |
| **Lokálna konzistencia** | ✅ Áno (Docker PostgreSQL) |
| **Aspire podpora** | ✅ Natívna (`AddPostgres()`) |
| **Backup** | Automatický (7-35 dní) |
| **High Availability** | Dostupné (za príplatok) |
| **Scaling** | Vertical + Storage |
| **SSL/TLS** | ✅ Áno (default) |

**Prečo áno:**
- ✅ Najlacnejšia enterprise databáza
- ✅ Free tier pokrýva 24/7 prevádzku prvý rok
- ✅ Konzistentné prostredie (lokálne = Azure)
- ✅ Automatické backupy a updates
- ✅ Výborný výkon pre portfolio site

**Prečo nie:**
- ⚠️ Drahšie ako Azure SQL Basic (ale lepší výkon)
- ⚠️ Free tier len 12 mesiacov

---

### 2. Azure SQL Database

| Kritérium | Hodnota |
|-----------|---------|
| **Cena (min)** | ~$5/mes (Basic) alebo ~$15/mes (S0) |
| **Výkon (min)** | 5 DTU (Basic) |
| **Storage (min)** | 2 GB (Basic) |
| **Oqtane podpora** | ✅ Dobrá (Microsoft.Data.SqlClient) |
| **Lokálna konzistencia** | ⚠️ Nie (lokálne SQL Server alebo LocalDB) |
| **Aspire podpora** | ✅ Natívna (`AddSqlServer()`) |
| **Backup** | Automatický (7-35 dní) |
| **High Availability** | ✅ Built-in (všetky tiers) |
| **Scaling** | Vertical (DTU/vCore models) |
| **SSL/TLS** | ✅ Áno (default) |

**Prečo áno:**
- ✅ Najlacnejšia opcja (Basic $5/mes)
- ✅ Microsoft ecosystem (dobrá integrácia)
- ✅ Built-in HA aj v Basic tier

**Prečo nie:**
- ⚠️ Basic tier má veľmi slabý výkon (5 DTU)
- ❌ Lokálne development problematické (SQL Server vs Azure SQL)
- ⚠️ S0 tier (~$15/mes) je drahší ako PostgreSQL
- ⚠️ 2 GB storage limit v Basic tier

---

### 3. MySQL Flexible Server

| Kritérium | Hodnota |
|-----------|---------|
| **Cena (min)** | ~$12/mes (B1s) |
| **Výkon (min)** | 1 vCore, 1 GB RAM |
| **Storage (min)** | 20 GB |
| **Oqtane podpora** | ⚠️ Základná (MySQL.Data) |
| **Lokálna konzistencia** | ✅ Áno (Docker MySQL) |
| **Aspire podpora** | ✅ Natívna (`AddMySql()`) |
| **Backup** | Automatický (7-35 dní) |
| **High Availability** | Dostupné (za príplatok) |
| **Scaling** | Vertical + Storage |
| **SSL/TLS** | ✅ Áno (default) |

**Prečo áno:**
- ✅ Podobná cena ako PostgreSQL
- ✅ Konzistentné prostredie (lokálne = Azure)

**Prečo nie:**
- ⚠️ Oqtane má slabšiu podporu pre MySQL
- ⚠️ Menej funkcií ako PostgreSQL
- ⚠️ 1 GB RAM v B1s (PostgreSQL má 2 GB za rovnakú cenu)

---

### 4. Cosmos DB ❌ NEODPORÚČAM

| Kritérium | Hodnota |
|-----------|---------|
| **Cena (min)** | ~$25/mes (Serverless) alebo ~$24/mes (Provisioned) |
| **Výkon (min)** | 400 RU/s (Provisioned) |
| **Storage (min)** | Pay-per-GB |
| **Oqtane podpora** | ❌ Žiadna (NoSQL) |
| **Lokálna konzistencia** | ⚠️ Emulator (obmedzený) |
| **Aspire podpora** | ✅ Natívna (`AddAzureCosmosDB()`) |

**Prečo nie:**
- ❌ Oqtane je navrhnuté pre relačné databázy
- ❌ Značne drahšie
- ❌ Overkill pre CMS
- ❌ Komplexná konfigurácia

---

## 💰 Cenové porovnanie (mesačne)

| Databáza | Tier | vCores | RAM | Storage | Cena/mes |
|----------|------|--------|-----|---------|----------|
| **PostgreSQL Flexible** | B1ms | 1 | 2 GB | 32 GB | **~$12-15** |
| **PostgreSQL Flexible** | Free* | 1 | 2 GB | 32 GB | **$0** |
| Azure SQL | Basic | - | - | 2 GB | ~$5 |
| Azure SQL | S0 | - | - | 250 GB | ~$15 |
| MySQL Flexible | B1s | 1 | 1 GB | 20 GB | ~$12 |
| Cosmos DB | Serverless | - | - | Pay-per-GB | ~$25+ |

\* Free tier: 750 hodín/mesiac prvý rok (pokrýva 24/7 prevádzku)

---

## 🔍 Use Case: Art Portfolio

Pre **ArtPortfolio** webové portfólio ilustrátora:

### Požiadavky:
- Stredný traffic (~1000 návštev/deň)
- Veľké obrázky (up to 10MB)
- Galérie s 100-500 artwork items
- Admin panel pre správu obsahu
- Contact forms, commission requests
- Blog/News section

### Odporúčanie: PostgreSQL Flexible Server (B1ms)

**Prečo:**
1. **Výkon**: 1 vCore + 2 GB RAM stačí pre stredný traffic
2. **Storage**: 32 GB pre databázu + Blob Storage pre obrázky
3. **Cena**: $12/mes alebo FREE prvý rok
4. **Škálovateľnosť**: Ľahko upgradovať na B2s/B4ms ak potrebné
5. **Development**: Rovnaké prostredie lokálne aj v Azure

### Alternatíva: Azure SQL S0

**Ak:**
- Preferujete Microsoft SQL Server
- Potrebujete built-in HA
- Storage 250 GB je benefit

**Ale:**
- Drahšie (~$15/mes)
- Komplikovanejší lokálny development

---

## 🚀 Scaling stratégia

### Fáza 1: Spustenie (0-1000 návštev/deň)
**PostgreSQL B1ms** (~$12/mes)
- 1 vCore, 2 GB RAM
- 32 GB storage
- Stačí pre prvé mesiace/roky

### Fáza 2: Rast (1000-10000 návštev/deň)
**PostgreSQL B2s** (~$25/mes)
- 2 vCores, 4 GB RAM
- 64 GB storage
- Horizontal scaling (Container Apps)

### Fáza 3: Scale-out (10000+ návštev/deň)
**PostgreSQL General Purpose** (~$100/mes)
- 4 vCores, 16 GB RAM
- 128 GB storage
- Read replicas
- CDN pre static content

---

## 📈 Estimovaný ROI

### Scenár: Portfolio s 500 návštevami/deň

**PostgreSQL Flexible (B1ms):**
- Database: $12/mes
- Container Apps: $15/mes (Free tier možný)
- Blob Storage: $2/mes (100 GB obrázkov)
- **Total: ~$29/mes** (alebo $17/mes s free tiers)

**Azure SQL (S0):**
- Database: $15/mes
- Container Apps: $15/mes
- Blob Storage: $2/mes
- **Total: ~$32/mes**

**Rozdiel: $3/mes** (PostgreSQL lacnejšie)
**Ročne: $36** úspora

---

## ✅ Finálne odporúčanie

### Pre ArtPortfolio:

1. **Prvý rok**: PostgreSQL Flexible Server **FREE tier**
   - $0/mesiac prvých 12 mesiacov
   - 750 hodín = 24/7 prevádzka zadarmo

2. **Po prvom roku**: PostgreSQL Flexible Server **B1ms**
   - $12-15/mesiac
   - Stačí pre 1000-5000 návštev/deň
   - Upgrade na B2s ak potrebné

3. **Lokálny development**: PostgreSQL v Dockeri (cez Aspire)
   - Konzistentné prostredie
   - Zero-config setup
   - pgAdmin zadarmo

### Prečo NIE Azure SQL Basic?

Aj keď je lacnejšie ($5/mes), má veľmi slabý výkon:
- ⚠️ 5 DTU = ~0.3 vCore ekvivalent
- ⚠️ 2 GB storage limit
- ⚠️ Pomalé queries pri väčších galériách

**PostgreSQL B1ms** za dvojnásobnú cenu ponúka **10x lepší výkon**.

---

## 📚 Ďalšie zdroje

- [Azure PostgreSQL Pricing Calculator](https://azure.microsoft.com/pricing/calculator/)
- [Azure SQL Pricing](https://azure.microsoft.com/pricing/details/azure-sql-database/)
- [Oqtane Database Providers](https://github.com/oqtane/oqtane.databases)
- [docs/AZURE_POSTGRESQL.md](AZURE_POSTGRESQL.md) - Deployment guide

---

**Aktualizované**: 2025-01-XX  
**Autor**: ArtPortfolio Team  
**Verzia**: 1.0
