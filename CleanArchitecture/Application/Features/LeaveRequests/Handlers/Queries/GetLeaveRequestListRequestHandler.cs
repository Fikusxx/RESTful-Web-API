using Application.DTOs;
using Application.Persistence.Contracts;
using AutoMapper;
using MediatR;


namespace Application.Features;


public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDTO>>
{
    private readonly ILeaveRequestRepository db;
    private readonly IMapper mapper;

    public GetLeaveRequestListRequestHandler(ILeaveRequestRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<LeaveRequestListDTO>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
    {
        var leaveRequests = await db.GetLeaveRequestsWithDetails();
        var leaveRequestsDTO = mapper.Map<List<LeaveRequestListDTO>>(leaveRequests);

        return leaveRequestsDTO;
    }
}
