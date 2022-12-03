using Application.DTOs;
using Application.Responses;
using MediatR;


namespace Application.Features;

public class UpdateLeaveAllocationCommand : IRequest<BaseCommandResponse>
{
    public UpdateLeaveAllocationDTO UpdateLeaveAllocationDTO { get; set; }
}
