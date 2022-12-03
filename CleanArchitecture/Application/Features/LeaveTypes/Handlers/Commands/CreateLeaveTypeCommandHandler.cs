using Application.DTOs;
using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features;


public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, BaseCommandResponse>
{
    private readonly ILeaveTypeRepository db;
    private readonly IMapper mapper;

    public CreateLeaveTypeCommandHandler(ILeaveTypeRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new CreateLeaveTypeDTOValidator();
        var validationResult = await validator.ValidateAsync(request.CreateLeaveTypeDTO);

        if (validationResult.IsValid == false)
        {
            response.Success = false;
            response.Message = "Creation failed";
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return response;
        }

        var leaveType = mapper.Map<LeaveType>(request.CreateLeaveTypeDTO);
        leaveType = await db.AddAsync(leaveType);

        response.Success = true;
        response.Message = "Creation successful";
        response.Id = leaveType.Id;

        return response;
    }
}
