using Application.DTOs;
using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features;


public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, BaseCommandResponse>
{
    private readonly ILeaveRequestRepository db;
    private readonly ILeaveTypeRepository leaveTypeRepository;
    private readonly IMapper mapper;

    public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository db, 
        IMapper mapper,
        ILeaveTypeRepository leaveTypeRepository)
    {
        this.db = db;
        this.mapper = mapper;
        this.leaveTypeRepository = leaveTypeRepository;
    }

    public async Task<BaseCommandResponse> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var leaveRequest = await db.GetAsync(request.Id);

        if (request.UpdateLeaveRequestDTO != null)
        {
            var validator = new UpdateLeaveRequestDTOValidator(leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDTO);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Update failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            mapper.Map(request.UpdateLeaveRequestDTO, leaveRequest);
        }
        else if (request.ChangeLeaveRequestApprovalDTO != null)
        {
            await db.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDTO.Approved);
        }

        await db.UpdateAsync(leaveRequest);

        response.Success = true;
        response.Message = "Update Successful";
        response.Id = request.Id;   

        return response;
    }
}

