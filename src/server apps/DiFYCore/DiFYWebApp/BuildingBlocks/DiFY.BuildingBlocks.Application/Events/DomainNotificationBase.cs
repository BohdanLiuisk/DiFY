﻿using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.BuildingBlocks.Application.Events
{
    public class DomainNotificationBase<T> : IDomainEventNotification<T>
        where T : IDomainEvent
    {
        public T DomainEvent { get; }

        public Guid Id { get; }

        public DomainNotificationBase(T domainEvent, Guid id)
        {
            DomainEvent = domainEvent;
            Id = id;
        }
    }
}