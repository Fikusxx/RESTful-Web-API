namespace WebAPI.Services.MappingService;

public interface IPropertyMappingService
{
    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
}