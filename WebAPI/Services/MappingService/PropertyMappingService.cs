using WebAPI.Models;
using WebAPI.Services.MappingService;

namespace WebAPI.Services;

public class PropertyMappingService : IPropertyMappingService
{
    private Dictionary<string, PropertyMappingValue> authorPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>{"Id"}) },
            { "MainCategory", new PropertyMappingValue(new List<string>{"MainCategory"}) },
            { "Age", new PropertyMappingValue(new List<string>{"DateOfBirth"}, true) },
            { "Name", new PropertyMappingValue(new List<string>{"FirstName", "LastName"}) }
        };

    private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

    public PropertyMappingService()
    {
        propertyMappings.Add(new PropertyMapping<AuthorDTO, Author>(authorPropertyMapping));
    }

    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
    {
        var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

        if (matchingMapping.Count() == 1)
        {
            return matchingMapping.First().MappingDictionary;
        }

        throw new Exception("Cannot find exact property mapping instance " + $"for <{typeof(TSource)},{typeof(TDestination)}");
    }
}

