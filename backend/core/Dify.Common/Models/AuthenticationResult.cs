using Dify.Common.Dto;

namespace Dify.Common.Models;

public class AuthenticationResult
{
    public AuthenticationResult(string authenticationError)
    {
        IsAuthenticated = false;
        AuthenticationError = authenticationError;
    }
    
    public AuthenticationResult(AuthenticatedUserDto userDto)
    {
        IsAuthenticated = true;
        UserDto = userDto;
    }
        
    public bool IsAuthenticated { get; }

    public string AuthenticationError { get; }
        
    public AuthenticatedUserDto UserDto { get; }
}

