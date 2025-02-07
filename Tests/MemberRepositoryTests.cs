using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

public class MemberRepositoryTests
{
    private async Task<EventDBContext> GetDBContext()
    {
        var options = new DbContextOptionsBuilder<EventDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new EventDBContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        return dbContext;

    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectMember()
    {
        var dbContext = await GetDBContext();
        var repository = new MemberRepository(dbContext); 
        var hash = "mqbk/Gsys55VXkiFcRx5nC5RSsXo1jFXNxAe4Gx7CCmqwmJFqNgTpwNTtLqPOTzhUBjHhsXnwZt2YjYPF/0LCA==";
        var salt = "EE7fa2AaSrMpAbbu5W/ysXeQWN/UvaKXdqbZm8s1mqCZ7lhd9I91FeQL9EaVZWVf2X593M9BGUVuW0tZlZ/h1erZMjzxebKTRcPnkH75IsPcDG1LUAhZa3XCIOZEmYLRLMP5CwD/zjnqKqCcsQiwCONVEI1B7v5wZRgVlhCGftw=";
        var member = new Member { Id = 1, Email = "test@example.com", RefreshToken = "refresh123", PasswordHash = hash, PasswordSalt = salt };
        await dbContext.Members.AddAsync(member);
        await dbContext.SaveChangesAsync();


        var result = await repository.GetByIdAsync(member.Id);


        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task AddAsync_SavesMemberSuccessfully()
    {
        var dbContext = await GetDBContext();
        var repository = new MemberRepository(dbContext);
        var hash = "mqbk/Gsys55VXkiFcRx5nC5RSsXo1jFXNxAe4Gx7CCmqwmJFqNgTpwNTtLqPOTzhUBjHhsXnwZt2YjYPF/0LCA==";
        var salt = "EE7fa2AaSrMpAbbu5W/ysXeQWN/UvaKXdqbZm8s1mqCZ7lhd9I91FeQL9EaVZWVf2X593M9BGUVuW0tZlZ/h1erZMjzxebKTRcPnkH75IsPcDG1LUAhZa3XCIOZEmYLRLMP5CwD/zjnqKqCcsQiwCONVEI1B7v5wZRgVlhCGftw=";
        var member = new Member { Id = 1, Email = "test@example.com", RefreshToken = "refresh123" , PasswordHash = hash, PasswordSalt = salt};


        await repository.AddAsync(member);
        var result = await dbContext.Members.FirstOrDefaultAsync(m => m.Id == 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("test@example.com", result.Email);
    }
}
