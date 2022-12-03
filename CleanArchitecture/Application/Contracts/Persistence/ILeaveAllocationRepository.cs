using Domain;


namespace Application.Persistence.Contracts;


public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
{
    public Task<LeaveAllocation> GetLeaveAllocationWithDetailsAsync(int id);
    public Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetailsAsync();
    public Task<bool> AllocationExistsAsync(string userId, int leaveTypeId, int period);
    public Task AddAllocationsAsync(List<LeaveAllocation> allocations);
    public Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId);
}
