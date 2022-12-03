using Application.DTOs;
using Application.Persistence.Contracts;
using AutoMapper;
using MediatR;

namespace Application.Features;


public class GetLeaveAllocationListRequestHandler : IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDTO>>
{
    private readonly ILeaveAllocationRepository db;
    private readonly IMapper mapper;

    public GetLeaveAllocationListRequestHandler(ILeaveAllocationRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<LeaveAllocationDTO>> Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
    {
        var leaveAllocations = await db.GetLeaveAllocationsWithDetailsAsync();
        var leaveAllocationDTOs = mapper.Map<List<LeaveAllocationDTO>>(leaveAllocations);

        return leaveAllocationDTOs;
    }
}
