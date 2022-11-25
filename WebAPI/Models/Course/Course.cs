using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class Course
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1500)]
    public string? Description { get; set; }

    public Author Author { get; set; }
    public Guid AuthorId { get; set; }
}
