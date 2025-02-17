using Application.DTOs.Response;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;

namespace Application.UseCases.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandler(IMemberRepository _memberRepository) : IRequestHandler<GetMemberByIdQuery, MemberDto>
{
    public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken) ?? throw new NotFoundException("Member");
        return member.Adapt<MemberDto>();
    }
}


