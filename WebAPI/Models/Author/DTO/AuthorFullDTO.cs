
namespace WebAPI.Models;


public class AuthorFullDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string MainCategory { get; set; }
}
