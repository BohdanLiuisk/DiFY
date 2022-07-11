namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventsBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;

        void Unsubscribe<T>() where T : IntegrationEvent;
    }
}