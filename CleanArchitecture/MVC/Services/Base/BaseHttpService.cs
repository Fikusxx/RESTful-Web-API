using CleanArchitecture.UI.MVC.Services;
using MVC.Contracts;
using System.Net.Http.Headers;

namespace MVC.Services.Base;

public class BaseHttpService
{
    protected readonly ILocalStorageService storage;
    protected IClient client;

    public BaseHttpService(ILocalStorageService storage, IClient client)
    {
        this.storage = storage;
        this.client = client;
    }

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
    {
        if (ex.StatusCode == 400)
        {
            return new Response<Guid> { Message = "Validation errors have occured", ValidationErrors = ex.Message, Success = false };
        }
        else if (ex.StatusCode == 404)
        {
            return new Response<Guid>() { Message = "The requested item could not be found.", Success = false };
        }
        else
        {
            return new Response<Guid>() { Message = "Something went wrong, please try again.", Success = false };
        }
    }

    protected void AddBearerToken()
    {
        if (storage.Exists("token"))
        {
            var header = new AuthenticationHeaderValue("Bearer", storage.GetStorageValue<string>("token"));
            client.HttpClient.DefaultRequestHeaders.Authorization = header;
        }
    }
}
