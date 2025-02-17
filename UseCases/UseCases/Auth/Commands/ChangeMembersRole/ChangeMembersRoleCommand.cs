using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Auth.Commands.ChangeMembersRole;

public class ChangeMembersRoleCommand : IRequest
{
    public int MemberId { get; set; }
    public Role Role { get; set; }

    public ChangeMembersRoleCommand(int memberId, Role role)
    {
        Role = role;
        MemberId = memberId;
    }
}
