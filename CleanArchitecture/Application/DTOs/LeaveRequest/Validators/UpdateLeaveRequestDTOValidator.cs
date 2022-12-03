using Application.Persistence.Contracts;
using FluentValidation;


namespace Application.DTOs;


public class UpdateLeaveRequestDTOValidator : AbstractValidator<UpdateLeaveRequestDTO>
{
    private readonly ILeaveTypeRepository db;

	public UpdateLeaveRequestDTOValidator(ILeaveTypeRepository db)
	{
		this.db = db;

		Include(new LeaveRequestDTOValidator(db));

        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than {ComparisonValue}");
    }

}
