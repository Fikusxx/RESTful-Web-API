using Application.DTOs;
using Application.Features;
using Application.Persistence.Contracts;
using Application.Profiles;
using AutoMapper;
using Moq;
using Shouldly;
using UnitTests.Mocks;

namespace UnitTests.LeaveTypes;


public class GetLeaveTypeListRequestHandlerTests
{
    private readonly IMapper mapper;
    private readonly Mock<ILeaveTypeRepository> db;

    public GetLeaveTypeListRequestHandlerTests()
    {
        db = MockLeaveTypeRepository.GetLeaveTypeRepository();

        var mapperConfig = new MapperConfiguration(x =>
        {
            x.AddProfile<MappingProfile>();
        });

        mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task GetLeaveTypeListTest()
    {
        var handler = new GetLeaveTypeListRequestHandler(db.Object, mapper);

        var result = await handler.Handle(new GetLeaveTypeListRequest(), CancellationToken.None);

        result.ShouldBeOfType<List<LeaveTypeDTO>>();

        result.Count.ShouldBe(2);
    }
}
