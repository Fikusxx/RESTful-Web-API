using Application.Persistence.Contracts;
using Domain;
using Moq;


namespace UnitTests.Mocks;


public static class MockLeaveTypeRepository
{
    public static Mock<ILeaveTypeRepository> GetLeaveTypeRepository()
    {
        var leaveTypes = new List<LeaveType>()
        {
            new LeaveType()
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation"
            },
            new LeaveType()
            {
                Id = 2,
                DefaultDays = 15,
                Name = "Test Sick"
            }
        };

        var mockRepo = new Mock<ILeaveTypeRepository>();

        mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(leaveTypes);

        mockRepo.Setup(x => x.AddAsync(It.IsAny<LeaveType>())).ReturnsAsync((LeaveType leaveType) =>
        {
            leaveTypes.Add(leaveType);
            return leaveType;
        });

        return mockRepo;
    }
}
