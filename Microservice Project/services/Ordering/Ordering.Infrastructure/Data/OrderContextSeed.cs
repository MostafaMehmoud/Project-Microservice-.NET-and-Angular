using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public  class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeded database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order> {

                new Order {
                    UserName = "mostafamehmoud",
                    FirstName = "Mostafa",
                    LastName = "Mehmoud",
                    EmailAddress ="mostafamehmoud51@gmail.com",
                    TotalPrice = 350,
                    AddressLine = "Cairo",
                    Country = "Egypt",
                    State = "Cairo",
                    ZipCode = "12345",
                    CardName = "Mostafa Mehmoud",
                    CardNumber = "1234567890123456",
                    Expiration = "12/25",
                    CVV = "123",
                    PaymentMethod = 1
                  
                }
            };
        }
    }
}
