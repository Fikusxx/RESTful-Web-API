using Application.Persistence.Contracts;
using FluentValidation;


namespace Application.DTOs;


public class CreateLeaveRequestDTOValidator : AbstractValidator<CreateLeaveRequestDTO>
{
	private readonly ILeaveTypeRepository db;

	public CreateLeaveRequestDTOValidator(ILeaveTypeRepository db)
	{
		this.db = db;

		Include(new LeaveRequestDTOValidator(db));
	}
}
