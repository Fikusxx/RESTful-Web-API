using AutoMapper;
using CleanArchitecture.UI.MVC.Services;
using MVC.Models;
using MVC.Models.Account;

namespace MVC;


public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<CreateLeaveTypeDTO, CreateLeaveTypeVM>().ReverseMap();
		CreateMap<LeaveTypeDTO, LeaveTypeVM>().ReverseMap();
		CreateMap<RegisterVM, RegistrationRequest>().ReverseMap();
	}
}
