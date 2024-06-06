
using System.ComponentModel.DataAnnotations;

namespace FufosEntities.Utilities
{
    public static class DataAnnotationsValidator
    {
        public static bool Validate<T>(T obj, ref List<ValidationResult> Errors)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(obj, context, Errors, validateAllProperties: true);

            return isValid;
        }
    }
}