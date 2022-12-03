using Application.Contracts;
using Application.DTOs;
using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features;


public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
{
    private readonly ILeaveAllocationRepository db;
    private readonly ILeaveTypeRepository leaveTypeRepository;
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository db,
        IMapper mapper,
        ILeaveTypeRepository leaveTypeRepository,
        IUserService userService)
    {
        this.db = db;
        this.mapper = mapper;
        this.leaveTypeRepository = leaveTypeRepository;
        this.userService = userService;
    }

    public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new CreateLeaveAllocationDTOValidator(leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request.CreateLeaveAllocationDTO);

        if (validationResult.IsValid == false)
        {
            response.Success = false;
            response.Message = "Creation failed";
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return response;
        }

        var leaveType = await leaveTypeRepository.GetAsync(request.CreateLeaveAllocationDTO.LeaveTypeId);
        var employees = await userService.GetEmployees();
        var period = DateTime.Now.Year;
        var allocations = new List<LeaveAllocation>();

        foreach (var emp in employees)
        {
            if (await db.AllocationExistsAsync(emp.Id, leaveType.Id, period))
                continue;

            allocations.Add(new LeaveAllocation()
            {
                EmployeeId = emp.Id,
                LeaveTypeId = leaveType.Id,
                NumberOfDays = leaveType.DefaultDays,
                Period = period
            });
        }

        await db.AddAllocationsAsync(allocations);

        response.Success = true;
        response.Message = "Creation successful";

        return response;
    }
}
