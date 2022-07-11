using System;
using DiFY.Modules.Administration.Application.Contracts;

namespace DiFY.Modules.Administration.Application.Members.CreateMember
{
    public class CreateMemberCommand : CommandBase<Guid>
    {
        public CreateMemberCommand(
            Guid id,
            Guid memberId,
            string login,
            string email,
            string firstName,
            string lastName,
            string name)
            : base(id)
        {
            Login = login;
            MemberId = memberId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Name = name;
        }

        internal Guid MemberId { get; }

        internal string Login { get; }

        internal string Email { get; }

        internal string FirstName { get; }

        internal string LastName { get; }

        internal string Name { get; }
    }
}