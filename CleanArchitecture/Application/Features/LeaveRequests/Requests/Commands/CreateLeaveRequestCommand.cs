using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features;


public class CreateLeaveRequestCommand : IRequest<BaseCommandResponse>
{
    public CreateLeaveRequestDTO CreateLeaveRequestDTO { get; set; }
}
