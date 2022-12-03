using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Profiles;


public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<LeaveRequest, LeaveRequestDTO>().ReverseMap();
		CreateMap<LeaveRequest, LeaveRequestListDTO>().ReverseMap();	
		CreateMap<LeaveRequest, CreateLeaveRequestDTO>().ReverseMap();	
		CreateMap<LeaveRequest, UpdateLeaveRequestDTO>().ReverseMap();	

		CreateMap<LeaveAllocation, LeaveAllocationDTO>().ReverseMap();
		CreateMap<LeaveAllocation, CreateLeaveAllocationDTO>().ReverseMap();
		CreateMap<LeaveAllocation, UpdateLeaveAllocationDTO>().ReverseMap();

		CreateMap<LeaveType, LeaveTypeDTO>().ReverseMap();
		CreateMap<LeaveType, CreateLeaveTypeDTO>().ReverseMap();
	}
}
