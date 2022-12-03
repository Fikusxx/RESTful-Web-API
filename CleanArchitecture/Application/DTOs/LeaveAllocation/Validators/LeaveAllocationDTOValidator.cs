using Application.Persistence.Contracts;
using FluentValidation;


namespace Application.DTOs;

public class LeaveAllocationDTOValidator : AbstractValidator<ILeaveAllocationDTO>
{
    private readonly ILeaveTypeRepository _db;

	public LeaveAllocationDTOValidator(ILeaveTypeRepository db)
	{
        _db = db;

		RuleFor(x => x.NumberOfDays).NotEmpty().WithMessage("{PropertyName} is required");
		RuleFor(x => x.NumberOfDays).GreaterThan(0).WithMessage("{PropertyName} must be at least 1");

        RuleFor(x => x.Period).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.Period).GreaterThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("{PropertyName} must be equal or after {ComparisonValue}");

        RuleFor(x => x.LeaveTypeId).GreaterThan(0)
        .MustAsync(async (id, token) =>
        {
            bool exists = await _db.Exists(id);
            return exists;
        }).WithMessage("{PropertyName} does not exist");
    }
}
