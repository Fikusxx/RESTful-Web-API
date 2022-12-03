using Application.DTOs;
using Application.Features;
using Application.Persistence.Contracts;
using Application.Profiles;
using Application.Responses;
using AutoMapper;
using Moq;
using Shouldly;
using UnitTests.Mocks;

namespace UnitTests.LeaveTypes;

public class CreateLeaveTypeCommandHandlerTests
{
    private readonly IMapper mapper;
    private readonly Mock<ILeaveTypeRepository> db;
    private readonly CreateLeaveTypeDTO createLeaveTypeDTO;

    public CreateLeaveTypeCommandHandlerTests()
    {
        db = MockLeaveTypeRepository.GetLeaveTypeRepository();

        var mapperConfig = new MapperConfiguration(x =>
        {
            x.AddProfile<MappingProfile>();
        });

        mapper = mapperConfig.CreateMapper();

        createLeaveTypeDTO = new CreateLeaveTypeDTO()
        {
            DefaultDays = 15,
            Name = "Test DTO"
        };
    }

    [Fact]
    public async Task CreateLeaveTypeTest()
    {
        var handler = new CreateLeaveTypeCommandHandler(db.Object, mapper);

        var command = new CreateLeaveTypeCommand() { CreateLeaveTypeDTO = createLeaveTypeDTO };

        var result = await handler.Handle(command, CancellationToken.None);

        var leaveTypes = await db.Object.GetAllAsync();

        leaveTypes.Count.ShouldBe(3);
        leaveTypes.Count.ShouldBeGreaterThan(2);

        result.ShouldBeOfType<BaseCommandResponse>();
    }
}
