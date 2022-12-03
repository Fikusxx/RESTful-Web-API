using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features;

public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, BaseCommandResponse>
{
    private readonly ILeaveRequestRepository db;
    private readonly IMapper mapper;

    public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var leaveRequest = await db.GetAsync(request.Id);

        if (leaveRequest == null)
        {
            response.Success = false;
            response.Message = "Delete failed";
            return response;
        }

        await db.DeleteAsync(leaveRequest);

        response.Success = true;
        response.Message = "Delete successful";

        return response;
    }
}
