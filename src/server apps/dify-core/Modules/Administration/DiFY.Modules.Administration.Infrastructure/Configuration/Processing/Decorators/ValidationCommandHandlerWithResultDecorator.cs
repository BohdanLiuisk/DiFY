using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application;
using DiFY.Modules.Administration.Application.Configuration.Commands;
using DiFY.Modules.Administration.Application.Contracts;
using FluentValidation;

namespace DiFY.Modules.Administration.Infrastructure.Configuration.Processing.Decorators
{
    internal class ValidationCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
    {
        private readonly IList<IValidator<T>> _validators;
        
        private readonly ICommandHandler<T, TResult> _decorated;

        public ValidationCommandHandlerWithResultDecorator(IList<IValidator<T>> validators, ICommandHandler<T, TResult> decorated)
        {
            _validators = validators;
            _decorated = decorated;
        }

        public Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var errors = _validators
                .Select(v => v.Validate(command))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (errors.Any())
            {
                throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
            }

            return _decorated.Handle(command, cancellationToken);
        }
    }
}