

namespace Application.DTOs;

public class LeaveRequestListDTO : BaseDTO
{
    public LeaveTypeDTO LeaveType { get; set; }
    public DateTime DateRequested { get; set; }
    public bool? Approved { get; set; }
}
