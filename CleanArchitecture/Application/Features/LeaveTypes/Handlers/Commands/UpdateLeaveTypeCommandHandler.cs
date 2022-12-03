using Application.DTOs;
using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features;


public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, BaseCommandResponse>
{
    private readonly ILeaveTypeRepository db;
    private readonly IMapper mapper;

    public UpdateLeaveTypeCommandHandler(ILeaveTypeRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new UpdateLeaveTypeDTOValidator();
        var validationResult = await validator.ValidateAsync(request.LeaveTypeDTO);

        if (validationResult.IsValid == false)
        {
            response.Success = false;
            response.Message = "Update failed";
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return response;
        }

        var leaveType = await db.GetAsync(request.LeaveTypeDTO.Id);
        mapper.Map(request.LeaveTypeDTO, leaveType);
        await db.UpdateAsync(leaveType);

        response.Success = true;
        response.Message = "Update successful";
        response.Id = leaveType.Id;

        return response;
    }
}
