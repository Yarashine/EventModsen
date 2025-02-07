using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Domain.Interfaces;
using Moq;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Mapster;

namespace Tests;

public class MemberServiceTests
{
    private readonly MemberUseCase _memberService;
    private readonly Mock<IMemberRepository> _mockMemberRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;

    public MemberServiceTests()
    {
        _mockMemberRepository = new Mock<IMemberRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
        _memberService = new MemberUseCase(_mockMemberRepository.Object, _mockEventRepository.Object);
    }

    [Fact]
    public async Task GetAllMembersByEvent_ShouldReturnMembers_WhenEventExists()
    {
        var eventId = 1;
        var mockEvent = new Event { Id = eventId };
        var members = new List<Member>
        {
            new() { Id = 1, Name = "Member 1" },
            new() { Id = 2, Name = "Member 2" }
        };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(mockEvent);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);


        var result = await _memberService.GetAllMembersByEvent(eventId);


        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Member 1", result.First().Name);
    }

    [Fact]
    public async Task GetAllMembersByEvent_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 1;

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _memberService.GetAllMembersByEvent(eventId));
    }

    [Fact]
    public async Task GetMemberById_ShouldReturnMember_WhenMemberExists()
    {
        var memberId = 1;
        var mockMember = new Member { Id = memberId, Name = "Member 1" };

        _mockMemberRepository.Setup(r => r.GetByIdAsync(memberId, CancellationToken.None)).ReturnsAsync(mockMember);


        var result = await _memberService.GetMemberById(memberId);


        Assert.NotNull(result);
        Assert.Equal("Member 1", result.Name);
    }

    [Fact]
    public async Task GetMemberById_ShouldThrowNotFoundException_WhenMemberDoesNotExist()
    {
        var memberId = 1;

        _mockMemberRepository.Setup(r => r.GetByIdAsync(memberId, CancellationToken.None)).ReturnsAsync((Member)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _memberService.GetMemberById(memberId));
    }

    [Fact]
    public async Task AddToEvent_ShouldAddMember_WhenEventAndMemberExistAndMemberNotInEvent()
    {
        var eventId = 1;
        var memberId = 2;
        var mockEvent = new Event { Id = eventId, MaxCountMembers = 5 };
        var members = new List<Member>();
        var member = new Member { Id = memberId };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(mockEvent);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);
        _mockMemberRepository.Setup(r => r.GetByIdAsync(memberId, CancellationToken.None)).ReturnsAsync(member);
        _mockMemberRepository.Setup(r => r.AddToEventAsync(memberId, eventId, CancellationToken.None)).Returns(Task.CompletedTask);


        await _memberService.AddToEvent(memberId, eventId);

        _mockMemberRepository.Verify(r => r.AddToEventAsync(memberId, eventId, CancellationToken.None), Times.Once);

    }

    [Fact]
    public async Task AddToEvent_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 1;
        var memberId = 2;

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _memberService.AddToEvent(memberId, eventId));
    }
    [Fact]
    public async Task AddToEvent_ShouldThrowBadRequestException_WhenMemberAlreadyInEvent()
    {
        var eventId = 1;
        var memberId = 2;
        var mockEvent = new Event { Id = eventId };
        var members = new List<Member>() { new() { Id = memberId } };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(mockEvent);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);

        await Assert.ThrowsAsync<BadRequestException>(() => _memberService.AddToEvent(memberId, eventId));
    }

    [Fact]
    public async Task AddToEvent_ShouldThrowNotFoundException_WhenMemberDoesNotExist()
    {

        var eventId = 1;
        var memberId = 2;
        var mockEvent = new Event { Id = eventId };
        var members = new List<Member>();

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(mockEvent);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);
        _mockMemberRepository.Setup(r => r.GetByIdAsync(memberId, CancellationToken.None)).ReturnsAsync((Member)null);


        await Assert.ThrowsAsync<NotFoundException>(() => _memberService.AddToEvent(memberId, eventId));
    }

    [Fact]
    public async Task DeleteMemberFromEvent_Success()
    {
        int memberId = 1, eventId = 2;
        var @event = new Event { Id = eventId };
        var members = new List<Member> { new Member { Id = memberId } };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(@event);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);
        _mockMemberRepository.Setup(r => r.DeleteFromEventAsync(memberId, eventId, CancellationToken.None)).Returns(Task.CompletedTask);

        await _memberService.DeleteMemberFromEvent(memberId, eventId);

        _mockMemberRepository.Verify(r => r.DeleteFromEventAsync(memberId, eventId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteMemberFromEvent_ThrowsNotFoundException_WhenEventNotFound()
    {
        int memberId = 1, eventId = 2;
        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _memberService.DeleteMemberFromEvent(memberId, eventId));
    }

    [Fact]
    public async Task DeleteMemberFromEvent_ThrowsBadRequestException_WhenMemberNotInEvent()
    {
        int memberId = 1, eventId = 2;
        var @event = new Event { Id = eventId };
        var members = new List<Member> { new Member { Id = 99 } };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(@event);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);

        await Assert.ThrowsAsync<BadRequestException>(() => _memberService.DeleteMemberFromEvent(memberId, eventId));
    }

    [Fact]
    public async Task DeleteMemberFromEvent_ThrowsNotFoundException_WhenNoMembersInEvent()
    {
        int memberId = 1, eventId = 2;
        var @event = new Event { Id = eventId };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(@event);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync((List<Member>)null);

        await Assert.ThrowsAsync<BadRequestException>(() => _memberService.DeleteMemberFromEvent(memberId, eventId));
    }

}