using Dify.Core.Application.Users.Commands;

namespace Dify.Core.Application.Users.Validators;

public class CreateNewUserCommandValidator : AbstractValidator<CreateNewUserCommand>
{
    public CreateNewUserCommandValidator()
    {
        RuleFor(v => v.Login)
            .MaximumLength(50)
            .NotEmpty();
        RuleFor(v => v.Password)
            .NotEmpty();
        RuleFor(v => v.FirstName)
            .MaximumLength(125)
            .NotEmpty();
        RuleFor(v => v.LastName)
            .MaximumLength(125)
            .NotEmpty();
        RuleFor(v => v.Email)
            .NotEmpty();
    }
}
