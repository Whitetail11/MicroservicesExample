namespace MicroservicesExample.Product.AsyncDataServices
{
    public interface IMessageBusService
    {
        void PublishNewProduct(Data.Entities.Product product);
    }
}
