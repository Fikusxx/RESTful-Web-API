using FluentValidation;


namespace Application.DTOs;

public class UpdateLeaveTypeDTOValidator : AbstractValidator<LeaveTypeDTO>
{
	public UpdateLeaveTypeDTOValidator()
	{
		Include(new LeaveTypeDTOValidator()); // name and default days

		RuleFor(x => x.Id).GreaterThan(0)
			.WithMessage("{PropertyName} must be greater than {ComparisonValue}");
	}
}
