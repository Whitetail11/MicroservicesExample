namespace MicroservicesExample.Order.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
