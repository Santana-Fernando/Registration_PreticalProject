using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Validation
{
    public class ValidationFields
    {
        private readonly object _fields;
        public ValidationFields(object fields)
        {
            _fields = fields;
        }

        public ValidationFieldsDTO IsValidWithErrors()
        {
            var validationContext = new ValidationContext(_fields, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(_fields, validationContext, validationResults, validateAllProperties: true);

            return new ValidationFieldsDTO
            {
                IsValid = isValid,
                ErrorMessages = validationResults
            };
        }
    }
}
