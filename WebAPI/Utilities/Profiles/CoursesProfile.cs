using AutoMapper;
using WebAPI.Models;

namespace WebAPI.Utilities;

public class CoursesProfile : Profile
{
	public CoursesProfile()
	{
		CreateMap<Course, CourseDTO>();

		CreateMap<CourseCreationDTO, Course>();

		CreateMap<CourseUpdateDTO, Course>().ReverseMap();
	}
}
