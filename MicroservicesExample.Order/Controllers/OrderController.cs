using MicroservicesExample.Order.Data;
using MicroservicesExample.Order.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesExample.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public OrderController(AppDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Data.Entities.Order>>> GetOrders()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Data.Entities.Order>>("order");

            if (cacheData != null)
            {
                return Ok(cacheData);
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);

            cacheData = await _context.Orders.Include(o => o.Products).ToListAsync();

            foreach (var item in cacheData)
            {
                item.Products = item.Products.Select(p => new Data.Entities.Product() { Description = p.Description, Name = p.Name, Id = p.Id, OrderId = p.OrderId, Price = p.Price }).ToList();
            }

            _cacheService.SetData("order", cacheData, expirationTime);

            return Ok(cacheData); 
        }

        [HttpPost]
        public async Task<ActionResult<Data.Entities.Order>> PostOrder([FromBody]ICollection<int> productsIds)
        {
            var order = new Data.Entities.Order();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var products = _context.Products.ToList().Where(p => productsIds.Any(i => i == p.Id));

            foreach (var product in products)
            {
                product.OrderId = order.Id;
            }
            _cacheService.RemoveData("order");

            await _context.SaveChangesAsync();

            return Ok(order.Id);
        }
    }
}
