using System;
using MediatR;

namespace DiFY.BuildingBlocks.Domain
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
        
        DateTime OccurredOn { get; }
    }
}