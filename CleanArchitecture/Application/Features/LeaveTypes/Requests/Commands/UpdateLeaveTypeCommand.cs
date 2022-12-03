using Application.DTOs;
using Application.Responses;
using MediatR;



namespace Application.Features;


public class UpdateLeaveTypeCommand : IRequest<BaseCommandResponse>
{
    public LeaveTypeDTO LeaveTypeDTO { get; set; }
}
