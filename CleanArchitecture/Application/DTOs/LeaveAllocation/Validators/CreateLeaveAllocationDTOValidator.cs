using Application.Persistence.Contracts;
using FluentValidation;


namespace Application.DTOs;

public class CreateLeaveAllocationDTOValidator : AbstractValidator<CreateLeaveAllocationDTO>
{
    private readonly ILeaveTypeRepository db;

	public CreateLeaveAllocationDTOValidator(ILeaveTypeRepository db)
	{
		this.db = db;

		RuleFor(x => x.LeaveTypeId)
			.GreaterThan(0)
			.MustAsync(async (id, token) =>
			{
				var exists = await this.db.Exists(id);
				return exists;
			})
			.WithMessage("{PropertyName} doesnt exist");
    }
}
