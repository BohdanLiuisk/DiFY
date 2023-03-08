using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DiFY.BuildingBlocks.Application.Data;
using DiFY.Modules.Social.Application.Configuration.Queries;

namespace DiFY.Modules.Social.Application.Members.GetUserProfile;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, GetUserProfileDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserProfileQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
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
        return await connection.QueryFirstAsync<GetUserProfileDto>(userProfileSelect, new { query.UserId });
    }
}
