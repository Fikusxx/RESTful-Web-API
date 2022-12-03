﻿

namespace Application.DTOs;

public class LeaveAllocationDTO : BaseDTO
{
    public int NumberOfDays { get; set; }
    public LeaveTypeDTO LeaveType { get; set; }
    public int LeaveTypeId { get; set; }
    public int Period { get; set; }
}
