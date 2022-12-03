using Application.DTOs;
using Application.Responses;
using MediatR;


namespace Application.Features;


public class CreateLeaveTypeCommand : IRequest<BaseCommandResponse>
{
    public CreateLeaveTypeDTO CreateLeaveTypeDTO { get; set; }
}
