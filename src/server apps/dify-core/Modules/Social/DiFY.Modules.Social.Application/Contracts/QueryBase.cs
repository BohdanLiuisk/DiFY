using System;
using DiFY.BuildingBlocks.Application.Queries;

namespace DiFY.Modules.Social.Application.Contracts
{
    public abstract class QueryBase<TResult> : IQuery<TResult>
    {
        protected QueryBase()
        {
            Id = Guid.NewGuid();
        }

        protected QueryBase(Guid id)
        {
            Id = id;
        }
        
        protected QueryBase(SortOption[] sortOptions)
        {
            SortOptions = sortOptions;
        }
        
        protected QueryBase(Guid id, SortOption[] sortOptions)
        {
            Id = id;
            SortOptions = sortOptions;
        }

        public Guid Id { get; }

        public SortOption[] SortOptions { get; }
    }
}
