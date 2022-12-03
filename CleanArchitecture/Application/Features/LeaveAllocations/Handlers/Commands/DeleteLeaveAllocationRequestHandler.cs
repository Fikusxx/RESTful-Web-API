using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features;

public class DeleteLeaveAllocationRequestHandler : IRequestHandler<DeleteLeaveAllocationCommand, BaseCommandResponse>
{
    private readonly ILeaveAllocationRepository db;
    private readonly IMapper mapper;

    public DeleteLeaveAllocationRequestHandler(ILeaveAllocationRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var leaveAllocation = await db.GetAsync(request.Id);

        if (leaveAllocation == null)
        {
            response.Success = false;
            response.Message = "Delete failed";
            return response;
        }

        await db.DeleteAsync(leaveAllocation);

        response.Success = true;
        response.Message = "Delete successful";

        return response;
    }
}
