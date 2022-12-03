

namespace Application.DTOs;

public interface ILeaveAllocationDTO
{
    public int NumberOfDays { get; set; }
    public int LeaveTypeId { get; set; }
    public int Period { get; set; }
}