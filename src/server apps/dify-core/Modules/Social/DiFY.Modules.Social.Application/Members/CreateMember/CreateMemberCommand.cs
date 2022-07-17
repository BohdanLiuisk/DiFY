using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Members.CreateMember;

public class CreateMemberCommand : CommandBase<Guid>
{
    public CreateMemberCommand(Guid id, Guid memberId, string login, string email,
        string firstName, string lastName, string name) : base(id) 
    {
        Login = login;
        MemberId = memberId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Name = name;
    }

    public Guid MemberId { get; }
    
    public string Login { get; }

    public string Email { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Name { get; }
}