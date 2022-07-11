using System.Threading;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Infrastructure.Interfaces;
using DiFY.Modules.Administration.Application.Configuration.Commands;
using DiFY.Modules.Administration.Application.Contracts;
using MediatR;

namespace DiFY.Modules.Administration.Infrastructure.Configuration.Processing.Decorators
{
    internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ICommandHandler<T> _decorated;
        
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommandHandlerDecorator(ICommandHandler<T> decorated, IUnitOfWork unitOfWork)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            await _decorated.Handle(command, cancellationToken);
            
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}