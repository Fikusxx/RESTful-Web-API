using Application.DTOs;
using Application.Persistence.Contracts;
using AutoMapper;
using MediatR;


namespace Application.Features;


public class GetLeaveTypeDetailsRequestHandler : IRequestHandler<GetLeaveTypeDetailsRequest, LeaveTypeDTO>
{
    private readonly ILeaveTypeRepository db;
    private readonly IMapper mapper;

    public GetLeaveTypeDetailsRequestHandler(ILeaveTypeRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<LeaveTypeDTO> Handle(GetLeaveTypeDetailsRequest request, CancellationToken cancellationToken)
    {
        var leaveType = await db.GetAsync(request.Id);
        var leaveTypeDTO = mapper.Map<LeaveTypeDTO>(leaveType);

        return leaveTypeDTO;
    }
}
