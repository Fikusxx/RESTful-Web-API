
namespace Application.DTOs;


public class LeaveTypeDTO : BaseDTO, ILeaveTypeDTO
{
    public string Name { get; set; }
    public int DefaultDays { get; set; }
}
