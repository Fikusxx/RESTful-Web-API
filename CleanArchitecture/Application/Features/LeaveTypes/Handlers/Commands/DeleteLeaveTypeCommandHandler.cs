using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features;

public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, BaseCommandResponse>
{
    private readonly ILeaveRequestRepository db;
    private readonly IMapper mapper;

    public DeleteLeaveTypeCommandHandler(ILeaveRequestRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var leaveType = await db.GetAsync(request.Id);

        if (leaveType == null)
        {
            response.Success = false;
            response.Message = "Delete failed";
            return response;
        }

        await db.DeleteAsync(leaveType);

        response.Success = true;
        response.Message = "Delete successful";

        return response;
    }
}
