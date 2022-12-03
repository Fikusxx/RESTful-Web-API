using Application.DTOs;
using Application.Responses;
using MediatR;


namespace Application.Features;


public class UpdateLeaveRequestCommand : IRequest<BaseCommandResponse>
{
    public int Id { get; set; } 
    public UpdateLeaveRequestDTO UpdateLeaveRequestDTO { get; set; }
    public ChangeLeaveRequestApprovalDTO ChangeLeaveRequestApprovalDTO { get; set; }
}
