using Application.Persistence.Contracts;
using FluentValidation;


namespace Application.DTOs;


public class UpdateLeaveAllocationDTOValidator : AbstractValidator<UpdateLeaveAllocationDTO>
{
    private readonly ILeaveTypeRepository db;

    public UpdateLeaveAllocationDTOValidator(ILeaveTypeRepository db)
    {
        this.db = db;

        Include(new LeaveAllocationDTOValidator(db));

        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than {ComparisonValue}");
    }
}
