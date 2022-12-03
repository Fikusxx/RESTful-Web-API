using Application.DTOs;
using Application.Persistence.Contracts;
using AutoMapper;
using MediatR;

namespace Application.Features;


public class GetLeaveRequestDetailsRequestHandler : IRequestHandler<GetLeaveRequestDetailsRequest, LeaveRequestDTO>
{
    private readonly ILeaveRequestRepository db;
    private readonly IMapper mapper;

    public GetLeaveRequestDetailsRequestHandler(ILeaveRequestRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<LeaveRequestDTO> Handle(GetLeaveRequestDetailsRequest request, CancellationToken cancellationToken)
    {
        var leaveRequest = await db.GetLeaveRequestWithDetails(request.Id);
        var leaveRequestDTO = mapper.Map<LeaveRequestDTO>(leaveRequest);

        return leaveRequestDTO;
    }
}
