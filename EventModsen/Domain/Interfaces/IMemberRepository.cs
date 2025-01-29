﻿namespace EventModsen.Domain.Interfaces;
using EventModsen.Infrastructure.DB.Models;

public interface IMemberRepository
{
    public Task<bool> AddToEventAsync(int memberId, int eventId);
    public Task<IEnumerable<Member>?> GetAllByEventIdAsync(int eventId);
    public Task<Member> GetByIdAsync(int id);
    public Task<bool> DeleteFromEventAsync(int memberId, int eventId);
}

