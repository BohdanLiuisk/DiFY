using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Queries;
using DiFY.Modules.Social.Domain.Membership.Abstraction;

namespace DiFY.Modules.Social.Application.Members.GetUserProfile;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, GetUserProfileDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private readonly IMemberContext _memberContext;

    private readonly IRedisConnectionFactory _redisConnectionFactory;

    public GetUserProfileQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory, 
        IMemberContext memberContext,
        IRedisConnectionFactory redisConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _memberContext = memberContext;
        _redisConnectionFactory = redisConnectionFactory;
    }
    
    public async Task<GetUserProfileDto> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        const string userProfileSelect = "SELECT " +
                                         $"[profile].[Id] AS [{nameof(GetUserProfileDto.Id)}], " +
                                         $"[profile].[Login] AS [{nameof(GetUserProfileDto.Login)}], " +
                                         $"[profile].[Email] AS [{nameof(GetUserProfileDto.Email)}], " +
                                         $"[profile].[FirstName] AS [{nameof(GetUserProfileDto.FirstName)}], " +
                                         $"[profile].[LastName] AS [{nameof(GetUserProfileDto.LastName)}], " +
                                         $"[profile].[CreatedOn] AS [{nameof(GetUserProfileDto.CreatedOn)}], " +
                                         $"[profile].[AvatarUrl] AS [{nameof(GetUserProfileDto.AvatarUrl)}] " +
                                         $"FROM [social].[v_Profile] AS [profile]" +
                                         "WHERE [profile].[Id] = @UserId";
        var userProfile = await connection.QueryFirstAsync<GetUserProfileDto>(
            userProfileSelect, new { query.UserId });
        userProfile.CurrentUser = userProfile.Id == _memberContext.MemberId.Value;
        var redisDb = await _redisConnectionFactory.GetConnectionAsync();
        var onlineValue = await redisDb.GetStringAsync($"online-{query.UserId}");
        userProfile.Online = !string.IsNullOrEmpty(onlineValue);
        return userProfile;
    }
}
