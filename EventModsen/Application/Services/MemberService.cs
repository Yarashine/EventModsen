namespace EventModsen.Application.Services;
using EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MemberService : IMemberService
{
    public Task<bool> AddToEvent(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMemberFromEvent(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Member>?> GetAllMembers()
    {
        throw new NotImplementedException();
    }

    public Task<Member?> GetMemberById(int id)
    {
        throw new NotImplementedException();
    }
}
