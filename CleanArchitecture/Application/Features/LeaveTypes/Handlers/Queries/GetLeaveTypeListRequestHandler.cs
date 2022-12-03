using Application.DTOs;
using Application.Persistence.Contracts;
using AutoMapper;
using MediatR;

namespace Application.Features;


public class GetLeaveTypeListRequestHandler : IRequestHandler<GetLeaveTypeListRequest, List<LeaveTypeDTO>>
{
    private readonly ILeaveTypeRepository db;
    private readonly IMapper mapper;

    public GetLeaveTypeListRequestHandler(ILeaveTypeRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<LeaveTypeDTO>> Handle(GetLeaveTypeListRequest request, CancellationToken cancellationToken)
    {
        var leaveTypes = await db.GetAllAsync();
        var leaveTypesDTO = mapper.Map<List<LeaveTypeDTO>>(leaveTypes);

        return leaveTypesDTO;
    }
}
