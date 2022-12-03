using Application.Contracts.Infrastructure;
using Application.DTOs;
using Application.Models;
using Application.Persistence.Contracts;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Features;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
{
    private readonly ILeaveRequestRepository db;
    private readonly ILeaveTypeRepository leaveTypeRepository;
    private readonly ILeaveAllocationRepository leaveAllocationRepository;
    private readonly IMapper mapper;
    private readonly IEmailSender emailSender;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CreateLeaveRequestCommandHandler(ILeaveRequestRepository db,
        IMapper mapper,
        ILeaveTypeRepository leaveTypeRepository,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor,
        ILeaveAllocationRepository leaveAllocationRepository)
    {
        this.db = db;
        this.mapper = mapper;
        this.leaveTypeRepository = leaveTypeRepository;
        this.emailSender = emailSender;
        this.httpContextAccessor = httpContextAccessor;
        this.leaveAllocationRepository = leaveAllocationRepository;
    }

    public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new CreateLeaveRequestDTOValidator(leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request.CreateLeaveRequestDTO);
        var userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;

        var allocation = await leaveAllocationRepository.GetUserAllocations(userId, request.CreateLeaveRequestDTO.LeaveTypeId);
        int days = (int)(request.CreateLeaveRequestDTO.EndDate - request.CreateLeaveRequestDTO.StartDate).TotalDays;

        if (days > allocation.NumberOfDays)
        {
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                nameof(request.CreateLeaveRequestDTO.EndDate), "You do not have enough days for this request"));
        }

        if (validationResult.IsValid == false)
        {
            response.Success = false;
            response.Message = "Creation Failed";
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return response;
        }

        var leaveRequest = mapper.Map<LeaveRequest>(request.CreateLeaveRequestDTO);
        leaveRequest.RequestingEmployeeId = userId;
        leaveRequest = await db.AddAsync(leaveRequest);

        response.Success = true;
        response.Message = "Creation Successful";
        response.Id = leaveRequest.Id;

        var emailAddress = httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

        var email = new Email()
        {
            To = emailAddress,
            Body = $"Your leave request for {request.CreateLeaveRequestDTO.StartDate} to {request.CreateLeaveRequestDTO.EndDate} " +
            $"has been submitted successfuly",
            Subject = "Leave request submitted"
        };

        try
        {
            await emailSender.SendEmailAsync(email);
        }
        catch (Exception ex)
        {
            // log an error
        }

        return response;
    }
}
