using Application.Models;


namespace Application.Contracts;

public interface IUserService
{
    public Task<List<Employee>> GetEmployees();
}
