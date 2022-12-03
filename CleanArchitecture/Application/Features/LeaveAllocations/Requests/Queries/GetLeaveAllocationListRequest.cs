using Application.DTOs;
using MediatR;


namespace Application.Features;


public class GetLeaveAllocationListRequest : IRequest<List<LeaveAllocationDTO>>
{

}
