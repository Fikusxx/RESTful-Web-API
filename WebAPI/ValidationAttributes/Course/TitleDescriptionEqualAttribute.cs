using System.ComponentModel.DataAnnotations;
using WebAPI.Models;

namespace WebAPI.ValidationAttributes;


public class TitleDescriptionEqualAttribute : ValidationAttribute
{
    public TitleDescriptionEqualAttribute()
    { }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var course = validationContext.ObjectInstance as CourseManipulation;

        if (course != null && course.Title == course.Description)
        {
            return new ValidationResult("Provided description should be different from the title.",
                new[] { nameof(CourseManipulation) });
        }

        return ValidationResult.Success;
    }
}
