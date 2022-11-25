using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class AuthorCreationDTO
{
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    [MaxLength(50)]
    public string MainCategory { get; set; }

    public ICollection<CourseCreationDTO>? Courses { get; set; }
}
