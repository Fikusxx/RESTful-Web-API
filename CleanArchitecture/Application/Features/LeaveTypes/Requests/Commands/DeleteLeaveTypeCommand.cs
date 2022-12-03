using Application.Responses;
using MediatR;


namespace Application.Features;

public class DeleteLeaveTypeCommand : IRequest<BaseCommandResponse>
{
    public int Id { get; set; }
}
