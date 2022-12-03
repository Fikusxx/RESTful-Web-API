using MVC.Models;
using MVC.Services.Base;

namespace MVC.Contracts;

public interface ILeaveTypeService
{
    public Task<List<LeaveTypeVM>> GetLeaveTypes();
    public Task<LeaveTypeVM> GetLeaveTypeDetails(int id);
    public Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM createLeaveTypeVM);
    public Task<Response<int>> UpdateLeaveType(LeaveTypeVM updateLeaveTypeVM);
    public Task<Response<int>> DeleteLeaveType(int id);
}
