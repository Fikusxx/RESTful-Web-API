using Application.DTOs;
using MediatR;


namespace Application.Features;


public class GetLeaveAllocationDetailsRequest : IRequest<LeaveAllocationDTO>
{
    public int Id { get; set; }
}
