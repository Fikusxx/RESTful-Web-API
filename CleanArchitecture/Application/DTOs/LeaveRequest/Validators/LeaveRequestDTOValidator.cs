using Application.Persistence.Contracts;
using FluentValidation;


namespace Application.DTOs;

public class LeaveRequestDTOValidator : AbstractValidator<ILeaveRequestDTO>
{
	private readonly ILeaveTypeRepository _db;

	public LeaveRequestDTOValidator(ILeaveTypeRepository db)
	{
		_db = db;

		RuleFor(x => x.StartDate).LessThan(x => x.EndDate)
							.WithMessage("{PropertyName} must be before {ComparisonValue}");

		RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate)
							.WithMessage("{PropertyName} must be after {ComparisonValue}");

		RuleFor(x => x.LeaveTypeId).GreaterThan(0)
		.MustAsync(async (id, token) =>
		{
			bool exists = await _db.Exists(id);
			return exists;
		}).WithMessage("{PropertyName} does not exist");
	}
}
