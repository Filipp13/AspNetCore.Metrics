using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AspNetCore.Metrics
{
    public static class PrometheusMetricsServiceCollectionExtensions
    {
        public static IServiceCollection AddPrometheusMetrics(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var metrics = AppMetrics.CreateDefaultBuilder()
              .OutputMetrics.AsPrometheusPlainText()
              .OutputMetrics.AsPrometheusProtobuf()
              .Build();

            services.AddMetrics(metrics);

            var options = new MetricsWebHostOptions
            {
                EndpointOptions = endpointsOptions =>
                {
                    endpointsOptions.MetricsTextEndpointOutputFormatter =
              metrics
                .OutputMetricsFormatters
                .OfType<MetricsPrometheusTextOutputFormatter>()
                .First();
                    endpointsOptions.MetricsEndpointOutputFormatter =
              metrics
                .OutputMetricsFormatters
                .OfType<MetricsPrometheusProtobufOutputFormatter>()
                .First();

                }
            };

            services.AddMetricsReportingHostedService(options.UnobservedTaskExceptionHandler);
            services.AddMetricsEndpoints(options.EndpointOptions, configuration);

            services.AddMetricsTrackingMiddleware(options.TrackingMiddlewareOptions, configuration);
            services.AddSingleton<IStartupFilter, DefaultMetricsEndpointsStartupFilter>();

            services.AddMetricsTrackingMiddleware(configuration);
            services.AddSingleton<IStartupFilter, DefaultMetricsTrackingStartupFilter>();

            services.AddAppMetricsCollectors();

            return services;
        }
    }
}