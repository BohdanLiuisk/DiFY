namespace Dify.Core.Application.Users.Commands.AuthenticateCommand;

public class AuthenticationResult
{
    public AuthenticationResult(string authenticationError)
    {
        IsAuthenticated = false;
        AuthenticationError = authenticationError;
    }
    
    public AuthenticationResult(AuthenticatedUser user)
    {
        IsAuthenticated = true;
        User = user;
    }
        
    public bool IsAuthenticated { get; }

    public string AuthenticationError { get; }
        
    public AuthenticatedUser User { get; }
}

