using System;
using System.Text.Json.Serialization;
using MediatR;

namespace DiFY.BuildingBlocks.Infrastructure.EventBus
{
    public abstract class IntegrationEvent : INotification
    {
        [JsonInclude]
        public Guid Id { get; }

        [JsonInclude]
        public DateTime OccurredOn { get; }

        [Newtonsoft.Json.JsonConstructor]
        protected IntegrationEvent(Guid id, DateTime occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }
    }
}