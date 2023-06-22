using MicroservicesExample.Product.AsyncDataServices;
using MicroservicesExample.Product.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesExample.Product.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMessageBusService _messageBusService;

        public ProductController(AppDbContext context, IMessageBusService messageBusService)
        {
            _context = context;
            _messageBusService = messageBusService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Data.Entities.Product>>> GetProduct()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Data.Entities.Product>> PostProduct(Data.Entities.Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            try
            {
                _messageBusService.PublishNewProduct(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok(product.Id);
        }
    }
}
