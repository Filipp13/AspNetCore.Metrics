using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AspNetCore.Metrics.Tests
{
    [TestFixture(Category = "Integration")]
    public class PrometheusMetricsServiceCollectionExtensionsTests
    {
        private void ConfigureServices(IServiceCollection services, IConfiguration configuration) => services.AddPrometheusMetrics(configuration).AddControllers();

        private void Configure(IApplicationBuilder app) => app.UseRouting()
          .UseEndpoints(endpoints =>
          {
              endpoints.MapControllers();
          });

        [Test]
        public async Task Configure_WithProductionEnv_Returns_HTTP_OK()
        {
            var config = new ConfigurationBuilder().Build();
            using var host = await new HostBuilder()
              .ConfigureWebHost(webBuilder =>
              {
                  webBuilder
              .UseTestServer()
              .UseSetting(WebHostDefaults.EnvironmentKey, "Test")
              .ConfigureServices(serviceCollection => ConfigureServices(serviceCollection, config))
              .Configure(Configure);
              })
              .StartAsync();

            var response = await host.GetTestClient().GetStringAsync("/metrics-text");

            Assert.IsNotEmpty(response);
        }
    }
}