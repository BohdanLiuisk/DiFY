using FluentValidation;

namespace DiFY.Modules.UserAccess.Application.Authentication.Authenticate
{
    internal class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            RuleFor(x => x.Login).NotEmpty().WithMessage("Login cannot be empty");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}