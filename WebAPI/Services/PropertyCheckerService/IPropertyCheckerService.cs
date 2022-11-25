namespace WebAPI.Services;

public interface IPropertyCheckerService
{
    public bool TypeHasProperties<T>(string? fields);
}