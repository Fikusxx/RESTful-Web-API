using System.Dynamic;
using System.Reflection;

namespace WebAPI.Utilities;

public static class IEnumerableExtensions
{
    public static IEnumerable<ExpandoObject> ShapeDataCollection<TSource>(this IEnumerable<TSource> source, string? fields) where TSource : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var expandoObjectList = new List<ExpandoObject>();
        var propertyInfoList = new List<PropertyInfo>();

        if (string.IsNullOrWhiteSpace(fields)) // if we didnt get any fields passed
        {
            var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            propertyInfoList.AddRange(propertyInfos);
        }
        else
        {
            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(TSource).GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    throw new Exception($"Property {propertyName} was not found on {typeof(TSource)}");

                propertyInfoList.Add(propertyInfo);
            }
        }

        foreach (TSource sourceObject in source)
        {
            var dataShapedObject = new ExpandoObject();

            foreach (var propertyInfo in propertyInfoList)
            {
                var propertyValue = propertyInfo.GetValue(sourceObject);
                dataShapedObject.TryAdd(propertyInfo.Name, propertyValue);
            }

            expandoObjectList.Add(dataShapedObject);
        }

        return expandoObjectList;
    }
}
