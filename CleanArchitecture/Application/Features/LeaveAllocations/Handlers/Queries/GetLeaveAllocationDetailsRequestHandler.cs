using Application.DTOs;
using Application.Persistence.Contracts;
using AutoMapper;
using MediatR;

namespace Application.Features;


public class GetLeaveAllocationDetailsRequestHandler : IRequestHandler<GetLeaveAllocationDetailsRequest, LeaveAllocationDTO>
{
    private readonly ILeaveAllocationRepository db;
    private readonly IMapper mapper;

    public GetLeaveAllocationDetailsRequestHandler(ILeaveAllocationRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<LeaveAllocationDTO> Handle(GetLeaveAllocationDetailsRequest request, CancellationToken cancellationToken)
    {
        var leaveAllocation = await db.GetLeaveAllocationWithDetailsAsync(request.Id);
        var leaveAllocationDTO = mapper.Map<LeaveAllocationDTO>(leaveAllocation);

        return leaveAllocationDTO;
    }
}
