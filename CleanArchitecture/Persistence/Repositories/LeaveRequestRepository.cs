using Application.Persistence.Contracts;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;


public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
    private readonly ApplicationDbContext db;

    public LeaveRequestRepository(ApplicationDbContext db) : base(db)
    { 
        this.db = db;
    }

    public async Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool? value)
    {
        leaveRequest.Approved = value;
        db.Entry(leaveRequest).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
    {
        var leaveRequests = await db.LeaveRequests.Include(x => x.LeaveType).ToListAsync();

        return leaveRequests;
    }

    public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
    {
        var leaveRequest = await db.LeaveRequests.Include(x => x.LeaveType).FirstOrDefaultAsync(x => x.Id == id);

        return leaveRequest;
    }
}
