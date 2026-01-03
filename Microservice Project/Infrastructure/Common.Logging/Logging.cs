using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfiguraLogger => (Context, _LoggerConfiguration) =>
        {
            var evr = Context.HostingEnvironment;
            _LoggerConfiguration.MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", evr.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", evr.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
            .WriteTo.Console();
            if(Context.HostingEnvironment.IsDevelopment())
            {
                _LoggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                _LoggerConfiguration.MinimumLevel.Override("catalog", LogEventLevel.Debug);
                _LoggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                _LoggerConfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
            }
            //TODO: Add Seq or other Sinks  
            var elasticUri = Context.Configuration["ElasticConfiguration:Uri"];    
            if(!string.IsNullOrEmpty(elasticUri))
            {
             _LoggerConfiguration.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv8,
                 IndexFormat = $"ecommerce-logs-{DateTime.UtcNow:yyyy.MM.dd}",
                 MinimumLogEventLevel =LogEventLevel.Debug
             });
            }
        };
    }
}
