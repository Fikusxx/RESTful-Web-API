

using FluentValidation;

namespace Application.DTOs;

public class CreateLeaveTypeDTOValidator : AbstractValidator<CreateLeaveTypeDTO>
{
	public CreateLeaveTypeDTOValidator()
	{
		Include(new LeaveTypeDTOValidator());
	}
}
