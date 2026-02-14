var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL database for Oqtane
// Lokálne: Docker PostgreSQL container
// Azure: Azure Database for PostgreSQL Flexible Server

// Choose deployment mode from environment variable or configuration
var useAzure = builder.Configuration["UseAzurePostgreSQL"] == "true";

if (useAzure)
{
    // Azure PostgreSQL Flexible Server
    // Requires Azure subscription and provisioned PostgreSQL server
    var postgres = builder.AddAzurePostgresFlexibleServer("postgres");
    var postgresDb = postgres.AddDatabase("artportfolio");

    // Main web application with Oqtane
    builder.AddProject<Projects.ArtPortfolio_Web>("artportfolio-web")
        .WithReference(postgresDb)
        .WaitFor(postgresDb);
}
else
{
    // Local Docker PostgreSQL container
    var pgUsername = builder.AddParameter("pg-username");
    var pgPassword = builder.AddParameter("pg-password", secret: true);

    var postgres = builder.AddPostgres("postgres", pgUsername, pgPassword, port: 5432)
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent)
        .WithPgAdmin(configureContainer: (cc) =>
        {
            cc.WithLifetime(ContainerLifetime.Persistent);
            cc.WithHostPort(60751);
        });

    // Main Oqtane database
    var postgresDb = postgres.AddDatabase("artportfolio");

    // Main web application with Oqtane
    builder.AddProject<Projects.ArtPortfolio_Web>("artportfolio-web")
        .WithReference(postgresDb)
        .WaitFor(postgresDb);
}

await builder.Build().RunAsync();
