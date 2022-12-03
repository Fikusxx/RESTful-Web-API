using Application.DTOs;
using MediatR;


namespace Application.Features;


public class GetLeaveRequestListRequest : IRequest<List<LeaveRequestListDTO>>
{

}
