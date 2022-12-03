using Application.DTOs;
using Application.Responses;
using MediatR;


namespace Application.Features;


public class CreateLeaveAllocationCommand : IRequest<BaseCommandResponse>
{
    public CreateLeaveAllocationDTO CreateLeaveAllocationDTO { get; set; }
}
