using EventModsen.Domain.Entities;
using EventModsen.Infrastructure.DB;
using EventModsen.Infrastructure.DB.Repositories;
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
        var member = new Member { Id = 1, Email = "test@example.com", RefreshToken = "refresh123" };
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
        var member = new Member { Id = 1, Email = "test@example.com", RefreshToken = "refresh123" };


        await repository.AddAsync(member);
        var result = await dbContext.Members.FirstOrDefaultAsync(m => m.Id == 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("test@example.com", result.Email);
    }
}
