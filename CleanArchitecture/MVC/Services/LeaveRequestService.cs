using AutoMapper;
using CleanArchitecture.UI.MVC.Services;
using MVC.Contracts;
using MVC.Models;
using MVC.Services.Base;

namespace MVC.Services;

public class LeaveRequestService : BaseHttpService, ILeaveRequestService
{
    private readonly IMapper _mapper;

    public LeaveRequestService(IMapper mapper, IClient _client, ILocalStorageService localStorageService) : base(localStorageService, _client)
    {
        this._mapper = mapper;
    }

    public async Task ApproveLeaveRequest(int id, bool approved)
    {
        AddBearerToken();
        try
        {
            var request = new ChangeLeaveRequestApprovalDTO { Approved = approved, Id = id };
            await client.ChangeApprovalAsync(request);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
    {
        try
        {
            var response = new Response<int>();
            CreateLeaveRequestDTO createLeaveRequest = _mapper.Map<CreateLeaveRequestDTO>(leaveRequest);
            AddBearerToken();
            var apiResponse = await client.LeaveRequestPOSTAsync(createLeaveRequest);
            if (apiResponse.Success)
            {
                response.Data = apiResponse.Id;
                response.Success = true;
            }
            else
            {
                foreach (var error in apiResponse.Errors)
                {
                    response.ValidationErrors += error + Environment.NewLine;
                }
            }
            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }

    public Task DeleteLeaveRequest(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
    {
        AddBearerToken();
        var leaveRequests = await client.LeaveRequestAllAsync();

        var model = new AdminLeaveRequestViewVM
        {
            TotalRequests = leaveRequests.Count,
            ApprovedRequests = leaveRequests.Count(q => q.Approved == true),
            PendingRequests = leaveRequests.Count(q => q.Approved == null),
            RejectedRequests = leaveRequests.Count(q => q.Approved == false),
            LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
        };
        return model;
    }

    public async Task<LeaveRequestVM> GetLeaveRequest(int id)
    {
        AddBearerToken();
        var leaveRequest = await client.LeaveRequestGETAsync(id);
        return _mapper.Map<LeaveRequestVM>(leaveRequest);
    }

    public async Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequests()
    {
        AddBearerToken();
        var leaveRequests = await client.LeaveRequestAllAsync();
        var allocations = await client.LeaveAllocationAllAsync();
        var model = new EmployeeLeaveRequestViewVM
        {
            LeaveAllocations = _mapper.Map<List<LeaveAllocationVM>>(allocations),
            LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
        };

        return model;
    }
}

