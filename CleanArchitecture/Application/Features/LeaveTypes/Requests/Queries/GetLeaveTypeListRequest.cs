using Application.DTOs;
using MediatR;


namespace Application.Features;


public class GetLeaveTypeListRequest : IRequest<List<LeaveTypeDTO>>
{

}
