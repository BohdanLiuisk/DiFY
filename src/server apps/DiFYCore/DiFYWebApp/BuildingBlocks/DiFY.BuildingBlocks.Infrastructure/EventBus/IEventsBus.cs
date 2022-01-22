namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventsBus
    {
        void Publish(IntegrationEvent @event);
        

        void Subscribe<T, TH>(TH eventHandler) where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent;
    }
}