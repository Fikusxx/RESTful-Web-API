

namespace Application.DTOs;


public class UpdateLeaveAllocationDTO : BaseDTO, ILeaveAllocationDTO
{
    public int NumberOfDays { get; set; }
    public int LeaveTypeId { get; set; }
    public int Period { get; set; }
}
