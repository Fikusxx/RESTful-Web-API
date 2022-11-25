using System.ComponentModel.DataAnnotations;
using WebAPI.ValidationAttributes;

namespace WebAPI.Models;


[TitleDescriptionEqual]
public abstract class CourseManipulation
{
    [MaxLength(50)]
    public virtual string Title { get; set; }

    [MaxLength(1500)]
    public virtual string? Description { get; set; }
}
