# Метрики в формате Prometheus

Метрики конфигурируется с помощью appsettings.json  https://www.app-metrics.io/ , пример:

        {
          "MetricsOptions": {
            "DefaultContextLabel": "MyMvcApplication",
            "Enabled": true
          },
          "MetricsWebTrackingOptions": {
            "ApdexTrackingEnabled": true,
            "ApdexTSeconds": 0.1,
            "IgnoredHttpStatusCodes": [ 404 ],
            "IgnoredRoutesRegexPatterns": [],
            "OAuth2TrackingEnabled": true
          },
          "MetricEndpointsOptions": {
            "MetricsEndpointEnabled": true,
            "MetricsTextEndpointEnabled": true,    
            "EnvironmentInfoEndpointEnabled": true
          }
        }

Конечные точки по умолчанию
*  **/metrics** - снимок метрик binary
*  **/metrics-text** - снимок метрикв в текстовом формате
*  **/env** - информация о среде

DevOps обычно используют /metrics-text, не используемые можно отключить в конфиге MetricEndpointsOptions