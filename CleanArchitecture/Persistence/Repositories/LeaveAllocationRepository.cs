using Application.Persistence.Contracts;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;


public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    private readonly ApplicationDbContext db;

    public LeaveAllocationRepository(ApplicationDbContext db) : base(db)
    {
        this.db = db;
    }

    public async Task AddAllocationsAsync(List<LeaveAllocation> allocations)
    {
        await db.AddRangeAsync(allocations);
        await db.SaveChangesAsync();
    }

    public async Task<bool> AllocationExistsAsync(string userId, int leaveTypeId, int period)
    {
        var exists = await db.LeaveAllocations.AnyAsync(x => x.EmployeeId == userId
        && x.LeaveTypeId == leaveTypeId
        && x.Period == period);

        return exists;
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetailsAsync()
    {
        var leaveAllocations = await db.LeaveAllocations.Include(x => x.LeaveType).ToListAsync();

        return leaveAllocations;
    }

    public async Task<LeaveAllocation> GetLeaveAllocationWithDetailsAsync(int id)
    {
        var leaveAllocation = await db.LeaveAllocations.Include(x => x.LeaveType).FirstOrDefaultAsync(x => x.Id == id);

        return leaveAllocation;
    }

    public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
    {
        var leaveAllocation = await db.LeaveAllocations.FirstOrDefaultAsync(x => x.EmployeeId == userId
        && x.LeaveTypeId == leaveTypeId);

        return leaveAllocation;
    }
}
