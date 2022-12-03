using Application.DTOs;
using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features;


public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, BaseCommandResponse>
{
    private readonly ILeaveAllocationRepository db;
    private readonly ILeaveTypeRepository leaveTypeRepository;
    private readonly IMapper mapper;

    public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository db, 
        IMapper mapper,
        ILeaveTypeRepository leaveTypeRepository)
    {
        this.db = db;
        this.mapper = mapper;
        this.leaveTypeRepository = leaveTypeRepository;
    }

    public async Task<BaseCommandResponse> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new UpdateLeaveAllocationDTOValidator(leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request.UpdateLeaveAllocationDTO);

        if (validationResult.IsValid == false)
        {
            response.Success = false;
            response.Message = "Update failed";
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return response;
        }

        var leaveAllocation = await db.GetAsync(request.UpdateLeaveAllocationDTO.Id);
        mapper.Map(request.UpdateLeaveAllocationDTO, leaveAllocation);
        await db.UpdateAsync(leaveAllocation);

        response.Success = true;
        response.Message = "Update successful";
        response.Id = leaveAllocation.Id;

        return response;
    }
}
