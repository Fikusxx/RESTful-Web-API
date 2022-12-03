using Application.Responses;
using MediatR;


namespace Application.Features;

public class DeleteLeaveAllocationCommand : IRequest<BaseCommandResponse>
{
    public int Id { get; set; }
}
