
using Application.RepositoryInterfaces;
using Moq;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Application.UseCases.Members.Queries.GetMemberByEvent;
using Application.UseCases.Members.Queries.GetMemberById;
using Application.UseCases.Members.Commands.AddMemberToEvent;
using Application.UseCases.Members.Commands.RemoveMemberFromEvent;

namespace Tests;

public class MemberUseCaseTests
{
    private readonly Mock<IMemberRepository> _mockMemberRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;

    public MemberUseCaseTests()
    {
        _mockMemberRepository = new Mock<IMemberRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
    }

    [Fact]
    public async Task GetMembersByEventQueryHandler_ShouldReturnMembers_WhenEventExists()
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


        var handler = new GetMembersByEventQueryHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        var result = await handler.Handle(new GetMembersByEventQuery(eventId), CancellationToken.None);



        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Member 1", result.First().Name);
    }

    [Fact]
    public async Task GetMembersByEventQueryHandler_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 1;

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync((Event)null);

        var handler = new GetMembersByEventQueryHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetMembersByEventQuery(eventId), CancellationToken.None));

    }

    [Fact]
    public async Task GetMemberByIdQueryHandler_ShouldReturnMember_WhenMemberExists()
    {
        var memberId = 1;
        var mockMember = new Member { Id = memberId, Name = "Member 1" };

        _mockMemberRepository.Setup(r => r.GetByIdAsync(memberId, CancellationToken.None)).ReturnsAsync(mockMember);


        var handler = new GetMemberByIdQueryHandler(_mockMemberRepository.Object);

        var result = await handler.Handle(new GetMemberByIdQuery(memberId), CancellationToken.None);


        Assert.NotNull(result);
        Assert.Equal("Member 1", result.Name);
    }

    [Fact]
    public async Task GetMemberByIdQueryHandler_ShouldThrowNotFoundException_WhenMemberDoesNotExist()
    {
        var memberId = 1;

        _mockMemberRepository.Setup(r => r.GetByIdAsync(memberId, CancellationToken.None)).ReturnsAsync((Member)null);

        var handler = new GetMemberByIdQueryHandler(_mockMemberRepository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetMemberByIdQuery(memberId), CancellationToken.None));

    }

    [Fact]
    public async Task AddMemberToEventCommandHandler_ShouldAddMember_WhenEventAndMemberExistAndMemberNotInEvent()
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


        var handler = new AddMemberToEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await handler.Handle(new AddMemberToEventCommand(memberId, eventId), CancellationToken.None);


        _mockMemberRepository.Verify(r => r.AddToEventAsync(memberId, eventId, CancellationToken.None), Times.Once);

    }

    [Fact]
    public async Task AddMemberToEventCommandHandler_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 1;
        var memberId = 2;

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync((Event)null);

        var handler = new AddMemberToEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new AddMemberToEventCommand(memberId, eventId), CancellationToken.None));

    }
    [Fact]
    public async Task AddMemberToEventCommandHandler_ShouldThrowBadRequestException_WhenMemberAlreadyInEvent()
    {
        var eventId = 1;
        var memberId = 2;
        var mockEvent = new Event { Id = eventId };
        var members = new List<Member>() { new() { Id = memberId } };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(mockEvent);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);

        var handler = new AddMemberToEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new AddMemberToEventCommand(memberId, eventId), CancellationToken.None));
    }

    [Fact]
    public async Task AddMemberToEventCommandHandler_ShouldThrowNotFoundException_WhenMemberDoesNotExist()
    {

        var eventId = 1;
        var memberId = 2;
        var mockEvent = new Event { Id = eventId };
        var members = new List<Member>();

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(mockEvent);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);
        _mockMemberRepository.Setup(r => r.GetByIdAsync(memberId, CancellationToken.None)).ReturnsAsync((Member)null);


        var handler = new AddMemberToEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new AddMemberToEventCommand(memberId, eventId), CancellationToken.None));
    }

    [Fact]
    public async Task RemoveMemberFromEventCommand_Success()
    {
        int memberId = 1, eventId = 2;
        var @event = new Event { Id = eventId };
        var members = new List<Member> { new() { Id = memberId } };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(@event);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);
        _mockMemberRepository.Setup(r => r.DeleteFromEventAsync(memberId, eventId, CancellationToken.None)).Returns(Task.CompletedTask);

        var handler = new RemoveMemberFromEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await handler.Handle(new RemoveMemberFromEventCommand(memberId, eventId), CancellationToken.None);

        _mockMemberRepository.Verify(r => r.DeleteFromEventAsync(memberId, eventId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task RemoveMemberFromEventCommand_ThrowsNotFoundException_WhenEventNotFound()
    {
        int memberId = 1, eventId = 2;
        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync((Event)null);

        var handler = new RemoveMemberFromEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new RemoveMemberFromEventCommand(memberId, eventId), CancellationToken.None));

    }

    [Fact]
    public async Task RemoveMemberFromEventCommand_ThrowsBadRequestException_WhenMemberNotInEvent()
    {
        int memberId = 1, eventId = 2;
        var @event = new Event { Id = eventId };
        var members = new List<Member> { new() { Id = 99 } };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(@event);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync(members);

        var handler = new RemoveMemberFromEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new RemoveMemberFromEventCommand(memberId, eventId), CancellationToken.None));

    }

    [Fact]
    public async Task RemoveMemberFromEventCommand_ThrowsNotFoundException_WhenNoMembersInEvent()
    {
        int memberId = 1, eventId = 2;
        var @event = new Event { Id = eventId };

        _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, CancellationToken.None)).ReturnsAsync(@event);
        _mockMemberRepository.Setup(r => r.GetAllByEventIdAsync(eventId, CancellationToken.None)).ReturnsAsync((List<Member>)null);

        var handler = new RemoveMemberFromEventCommandHandler(_mockMemberRepository.Object, _mockEventRepository.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new RemoveMemberFromEventCommand(memberId, eventId), CancellationToken.None));

    }

}