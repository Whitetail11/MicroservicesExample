namespace MicroservicesExample.Order.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
