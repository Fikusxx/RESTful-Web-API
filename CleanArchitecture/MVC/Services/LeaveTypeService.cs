using AutoMapper;
using CleanArchitecture.UI.MVC.Services;
using MVC.Contracts;
using MVC.Models;
using MVC.Services.Base;

namespace MVC.Services;

public class LeaveTypeService : BaseHttpService, ILeaveTypeService
{
    private readonly IMapper mapper;

    public LeaveTypeService(ILocalStorageService storage, IClient client, IMapper mapper) : base(storage, client)
    {
        this.mapper = mapper;
    }

    public async Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM createLeaveTypeVM)
    {
        try
        {
            var response = new Response<int>();
            var createLeaveTypeDTO = mapper.Map<CreateLeaveTypeDTO>(createLeaveTypeVM);
            AddBearerToken();
            var apiResponse = await client.LeaveTypePOSTAsync(createLeaveTypeDTO);

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

    public async Task<Response<int>> DeleteLeaveType(int id)
    {
        try
        {
            var response = new Response<int>() { Success = true };
            AddBearerToken();
            await client.LeaveTypeDELETEAsync(id);

            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }

    public async Task<LeaveTypeVM> GetLeaveTypeDetails(int id)
    {
        AddBearerToken();
        var leaveType = await client.LeaveTypeGETAsync(id);
        var leaveTypeVM = mapper.Map<LeaveTypeVM>(leaveType);

        return leaveTypeVM;
    }

    public async Task<List<LeaveTypeVM>> GetLeaveTypes()
    {
        AddBearerToken();
        var leaveTypes = await client.LeaveTypeAllAsync();
        var leaveTypesVM = mapper.Map<List<LeaveTypeVM>>(leaveTypes);

        return leaveTypesVM;
    }

    public async Task<Response<int>> UpdateLeaveType(LeaveTypeVM updateLeaveTypeVM)
    {
        try
        {
            var response = new Response<int>() { Success = true };
            var updateLeaveTypeDTO = mapper.Map<LeaveTypeDTO>(updateLeaveTypeVM);
            AddBearerToken();
            await client.LeaveTypePUTAsync(updateLeaveTypeDTO);

            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }
}
