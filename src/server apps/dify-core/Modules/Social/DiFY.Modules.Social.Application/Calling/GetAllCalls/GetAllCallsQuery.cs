﻿using DiFY.BuildingBlocks.Application.Queries;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.GetAllCalls;

public class GetAllCallsQuery : QueryBase<GetAllCallsQueryResult>, IPagedQuery
{
    public GetAllCallsQuery(int? page, int? perPage, SortOption[] sortOptions) : base(sortOptions)
    {
        Page = page;
        PerPage = perPage;
    }
    
    public int? Page { get; }
    
    public int? PerPage { get; }
}