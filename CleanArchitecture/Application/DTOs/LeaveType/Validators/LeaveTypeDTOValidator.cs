using FluentValidation;


namespace Application.DTOs;

public class LeaveTypeDTOValidator : AbstractValidator<ILeaveTypeDTO>
{
	public LeaveTypeDTOValidator()
	{
		RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required");
		RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required");
		RuleFor(x => x.Name).MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");

		RuleFor(x => x.DefaultDays).NotEmpty().WithMessage("{PropertyName} is required");
		RuleFor(x => x.DefaultDays).NotNull();
		RuleFor(x => x.DefaultDays).GreaterThan(0).WithMessage("{PropertyName} must be at least 1");
		RuleFor(x => x.DefaultDays).LessThan(100).WithMessage("{PropertyName} must be less than 100");
	}
}
