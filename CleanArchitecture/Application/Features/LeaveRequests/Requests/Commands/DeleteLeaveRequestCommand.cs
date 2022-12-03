using Application.Responses;
using MediatR;


namespace Application.Features;


public class DeleteLeaveRequestCommand : IRequest<BaseCommandResponse>
{
    public int Id { get; set; } 
}
