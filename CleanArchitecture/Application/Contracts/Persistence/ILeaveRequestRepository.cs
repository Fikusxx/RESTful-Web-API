using Domain;


namespace Application.Persistence.Contracts;


public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
{
    public Task<LeaveRequest> GetLeaveRequestWithDetails(int id);
    public Task<List<LeaveRequest>> GetLeaveRequestsWithDetails();
    public Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool? value);
}
