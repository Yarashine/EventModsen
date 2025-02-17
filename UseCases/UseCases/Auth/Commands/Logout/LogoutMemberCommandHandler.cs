using Domain.Exceptions;
using Application.RepositoryInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Auth.Commands.Logout;

public class LogoutMemberCommandHandler(IMemberRepository _memberRepository) : IRequestHandler<LogoutMemberCommand>
{

    public async Task Handle(LogoutMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken) ?? throw new NotFoundException("Member");
        await _memberRepository.UpdateRefreshAsync(request.MemberId, null, cancellationToken);
    }
}

