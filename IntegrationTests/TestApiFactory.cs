using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infrastructure.Data;
using IntegrationTests.MockApis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using WebApplicationApi;

namespace IntegrationTests;

public class TestApiFactory : WebApplicationFactory<IApiMaker>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("localdevpassword#123")
        .Build();
    private MockChiliServer _chiliMockServer = new ();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_container.GetConnectionString()));
            services.AddHttpClient("myClient", config =>
            {
                config.BaseAddress = new Uri(_chiliMockServer.Url);
                config.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            _ = Seed.SeedUsers(context);
        });
    }

    public async Task InitializeAsync()
    {
        _chiliMockServer.StartServer();
        _chiliMockServer.SetupGetChiliApiKey();

        await _container.StartAsync();
    }
    
    public new Task DisposeAsync()
    {
        _chiliMockServer.Dispose();
        return _container.StartAsync();
    }
}