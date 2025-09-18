using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Orionexx.Identity.Infrastructure.Utilities;

public static class ValidateConfigurationHelper
{
    public static void ValidateSectionRecursive(object? obj)
    {
        var visited = new HashSet<object?>();
        ValidateSectionRecursive(obj, visited);
    }

    private static void ValidateSectionRecursive(object? obj, HashSet<object?> visited)
    {
        if (obj == null || !visited.Add(obj)) return;

        var context = new ValidationContext(obj);
        Validator.ValidateObject(obj, context, validateAllProperties: true);

        foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetIndexParameters().Length > 0)
                continue;

            var value = property.GetValue(obj);

            if (value == null || value is string || property.PropertyType.IsValueType)
                continue;

            ValidateSectionRecursive(value, visited);
        }
    }
}
