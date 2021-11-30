using System;
using DiFY.Modules.UserAccess.Application.Contracts;

namespace DiFY.Modules.UserAccess.Application.Configuration.Commands
{
    public class InternalCommandBase : ICommand
    {
        public Guid Id { get;}

        protected InternalCommandBase(Guid id)
        {
            this.Id = id;
        }
    }

    public abstract class InternalCommandBase<TResult> : ICommand<TResult>
    {
        public Guid Id { get; }

        protected InternalCommandBase()
        {
            this.Id = Guid.NewGuid();
        }

        protected InternalCommandBase(Guid id)
        {
            this.Id = id;
        }
    }
}