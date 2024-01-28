using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Reflection.Emit;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new() {
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    EmailAddress = "teste@teste.com",
                    AddressLine = "Av. Paulista",
                    State = "São Paulo",
                    Country = "Brasil",
                    ZipCode = "00000000",
                    TotalPrice = 350
                }
            };
        }
    }
}
