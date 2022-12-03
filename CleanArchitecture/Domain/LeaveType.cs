using Domain.Common;


namespace Domain;


public class LeaveType : BaseDomainEntity
{
    public string Name { get; set; }
    public int DefaultDays { get; set; }
}
