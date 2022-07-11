using System;
using RabbitMQ.Client;

namespace DiFY.BuildingBlocks.EventBus
{
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}