using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class Author
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public DateTime? DateOfDeath { get; set; }

    [Required]
    [MaxLength(50)]
    public string MainCategory { get; set; }

    public ICollection<Course>? Courses { get; set; }
}
