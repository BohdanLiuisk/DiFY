using System;

namespace DiFY.BuildingBlocks.Infrastructure.Interfaces
{
    public interface IDomainNotificationsMapper
    {
        string GetName(Type type);

        Type GetType(string name);
    }
}