using MVC.Services.Base;

namespace MVC.Contracts;

public interface ILeaveAllocationService
{
    public Task<Response<int>> CreateLeaveAllocations(int leaveTypeId);
}
