﻿using System;
using System.Threading;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.UserAccess.Application.Configuration.Commands;
using DiFY.Modules.UserAccess.Application.Contracts;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace DiFY.Modules.UserAccess.Infrastructure.Configuration.Processing.Decorators
{
    internal class LoggingCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ILogger _logger;
        
        private readonly IExecutionContextAccessor _executionContextAccessor;
        
        private readonly ICommandHandler<T> _decorated;

        public LoggingCommandHandlerDecorator(ILogger logger, IExecutionContextAccessor executionContextAccessor, ICommandHandler<T> decorated)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
            _decorated = decorated;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            using (LogContext.Push(new RequestLogEnricher(_executionContextAccessor), new CommandLogEnricher(command)))
            {
                try
                {
                    _logger.Information("Executing command {Command}", command.GetType().Name);

                    var result = await _decorated.Handle(command, cancellationToken);
                    
                    _logger.Information("Command {Command} processing successful.", command.GetType().Name);

                    return result;
                }
                catch (Exception exception)
                {
                    _logger.Error(exception,"Command {Command} processing failed.");
                    throw;
                }
            }
        }
        
        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly ICommand _command;

            public CommandLogEnricher(ICommand command)
            {
                _command = command;
            }
        
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty(
                    "Context", new ScalarValue($"Command: {_command.Id.ToString()}")));
            }
        }
    
        private class RequestLogEnricher : ILogEventEnricher
        {
            private readonly IExecutionContextAccessor _executionContextAccessor;

            public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
            {
                _executionContextAccessor = executionContextAccessor;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_executionContextAccessor.IsAvailable)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_executionContextAccessor.CorrelationId)));
                }
            }
        }
    }
}