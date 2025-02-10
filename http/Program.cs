using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using http.context;

var host = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        // Load the local.settings.json file if it exists
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        // Also load environment variables so that they override settings in the JSON file
        config.AddEnvironmentVariables();
    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        // Retrieve the connection string from the "ConnectionStrings" section
        string? connectionString = context.Configuration["ConnectionStrings:PostgresConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("The 'PostgresConnectionString' connection string is not set.");
        }

        // Register the ApplicationDbContext with the PostgreSQL provider
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Register telemetry services (as before)
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();


