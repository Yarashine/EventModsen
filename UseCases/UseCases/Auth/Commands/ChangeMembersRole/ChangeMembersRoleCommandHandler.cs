using Domain.Exceptions;
using Application.RepositoryInterfaces;
using MediatR;

namespace Application.UseCases.Auth.Commands.ChangeMembersRole;

public class ChangeMembersRoleCommandHandler(IMemberRepository _memberRepository) : IRequestHandler<ChangeMembersRoleCommand>
{
    public async Task Handle(ChangeMembersRoleCommand request, CancellationToken cancelToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancelToken) ?? throw new NotFoundException("Member");
        await _memberRepository.ChangeRole(request.MemberId, request.Role, cancelToken);
    }
}