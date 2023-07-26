using Dify.Core.Application.Common;
using Dify.Core.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Dify.Core.Application.Calls.EventHandlers;

public class NewCallCreatedEventHandler : INotificationHandler<NewCallCreatedEvent>
{
    private readonly IDifyContext _difyContext;
    
    private readonly ILogger<NewCallCreatedEventHandler> _logger;
    
    public NewCallCreatedEventHandler(IDifyContext difyContext, ILogger<NewCallCreatedEventHandler> logger)
    {
        _difyContext = difyContext;
        _logger = logger;
    }
    
    public Task Handle(NewCallCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"New call created: {notification.Call.Name}");
        return Task.CompletedTask;
    }
}
