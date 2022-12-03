using Application.DTOs;
using MediatR;


namespace Application.Features;


public class GetLeaveTypeDetailsRequest : IRequest<LeaveTypeDTO>
{
    public int Id { get; set; }
}
