using Dify.Core.Application.Common;

namespace Dify.Core.WebApi.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int UserId
    {
        get 
        {
            var subClaim = _httpContextAccessor.HttpContext?.User?.Claims?
                .SingleOrDefault(x => x.Type == "userId")?.Value;
            if (subClaim != null)
            {
                return int.Parse(subClaim);
            }
            throw new UnauthorizedAccessException("User context is not available.");
        }
    }
}
