using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrationDatabase<TContext>(this IHost host) 
        {
            using(var scoped = host.Services.CreateScope())
            {
                var serviceProvider = scoped.ServiceProvider;
                var config=serviceProvider.GetRequiredService<IConfiguration>();
                var loggar=serviceProvider.GetRequiredService<ILogger<TContext>>();
                try
                {
                    loggar.LogInformation("Discount migration Db started");
                    ApplyMigrations(config);
                    loggar.LogInformation("Discount migration Db completed");
                }
                catch(Exception ex) 
                {
                    loggar.LogError(ex, "Cannot Create Migration");
                    throw;
                }
            }
            return host;
        }

        private static void ApplyMigrations(IConfiguration config)
        {
            int retry = 5;
            while (retry > 0)
            {
                try
                {
                    var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var cmd = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText= @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                        ProductName VARCHAR(500) NOT NULL,
                                        Description TEXT,
                                        Amount INT

                    )";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES('IPhone X','IPhone Discount',150);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES('Samsung 10','Samsung Discount',100);";
                    cmd.ExecuteNonQuery();
                    break;
                }
                catch (Exception ex)
                {
                    retry--;
                    if (retry == 0)
                    {
                        throw;
                    }
                   Thread.Sleep(2000);
                }

            }
        }
    }
}
