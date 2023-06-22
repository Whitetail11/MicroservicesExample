using MicroservicesExample.Order.Data;
using MicroservicesExample.Order.Data.Entities;
using System.Text.Json;

namespace MicroservicesExample.Order.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }
        public void ProcessEvent(string message)
        {
            AddProduct(message);
        }

        private void AddProduct(string message)
        {
            var product = JsonSerializer.Deserialize<Product>(message);
            if (product is not null)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
        }
    }
}
