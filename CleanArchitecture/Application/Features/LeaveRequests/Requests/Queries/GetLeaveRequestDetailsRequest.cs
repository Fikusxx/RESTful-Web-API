

using Application.DTOs;
using MediatR;

namespace Application.Features;

public class GetLeaveRequestDetailsRequest : IRequest<LeaveRequestDTO>
{
    public int Id { get; set; }
}
