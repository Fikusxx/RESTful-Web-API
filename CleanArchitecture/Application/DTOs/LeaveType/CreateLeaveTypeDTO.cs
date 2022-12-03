

namespace Application.DTOs;


public class CreateLeaveTypeDTO : ILeaveTypeDTO
{
    public string Name { get; set; }
    public int DefaultDays { get; set; }
}
