


namespace Application.DTOs;


public class UpdateLeaveRequestDTO : BaseDTO, ILeaveRequestDTO
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int LeaveTypeId { get; set; }
    public string RequestComments { get; set; }
    public bool Cancelled { get; set; }
}
