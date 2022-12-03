using CleanArchitecture.UI.MVC.Services;
using MVC.Contracts;
using MVC.Services.Base;

namespace MVC.Services;

public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
{
    public LeaveAllocationService(ILocalStorageService storage, IClient client) : base(storage, client)
    { }

    public async Task<Response<int>> CreateLeaveAllocations(int leaveTypeId)
    {
        try
        {
            var response = new Response<int>();
            var createLeaveAllocation = new CreateLeaveAllocationDTO() { LeaveTypeId = leaveTypeId };
            AddBearerToken();

            var apiResponse = await client.LeaveAllocationPOSTAsync(createLeaveAllocation);
            
            if (apiResponse.Success)
            {
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
}
