using Application.Contracts;
using Application.Models;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services;


public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> userManager;

	public UserService(UserManager<ApplicationUser> userManager)
	{
		this.userManager = userManager;
	}

	public async Task<List<Employee>> GetEmployees()
	{
		var employeeUsers = await userManager.GetUsersInRoleAsync("Employee");
		var employees = employeeUsers.Select(x => new Employee()
		{
			Id = x.Id,
			Email = x.Email,
			FirstName = x.FirstName,
			LastName = x.LastName
		}).ToList();

		return employees;
	}
}
