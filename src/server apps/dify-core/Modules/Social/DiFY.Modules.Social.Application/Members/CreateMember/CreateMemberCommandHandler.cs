using System;
using System.Threading;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Configuration.Commands;
using DiFY.Modules.Social.Domain.Membership;
using DiFY.Modules.Social.Domain.Membership.Abstraction;

namespace DiFY.Modules.Social.Application.Members.CreateMember;

public class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Guid> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = Member.Create(request.MemberId, request.Login, request.Email, request.FirstName, 
            request.LastName, request.Name, DateTime.UtcNow);
        await _memberRepository.AddAsync(member);
        return member.Id.Value;
    }
}