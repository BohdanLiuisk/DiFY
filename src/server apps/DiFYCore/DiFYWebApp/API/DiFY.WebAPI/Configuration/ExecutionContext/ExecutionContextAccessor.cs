using System;
using System.Linq;
using DiFY.BuildingBlocks.Application;
using Microsoft.AspNetCore.Http;

namespace DiFY.WebAPI.Configuration.ExecutionContext
{
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User?.Claims?
                    .SingleOrDefault(claim => claim.Type == "sub")?.Value != null)
                {
                    return Guid.Parse(_httpContextAccessor.HttpContext.User.Claims
                        .Single(claim => claim.Type == "sub").Value);
                }

                throw new ApplicationException("User context is not available.");
            }
        }

        public Guid CorrelationId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && IsAvailable && _httpContextAccessor.HttpContext.Request.Headers.Keys
                    .Any(key => key == CorrelationMiddleware.CorrelationHeaderKey))
                {
                    return Guid.Parse(
                        _httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]);
                }

                throw new ApplicationException("Http context and correlation id is not available.");
            }
        }

        public bool IsAvailable => _httpContextAccessor.HttpContext != null;
    }
}