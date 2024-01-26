using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Validation
{
    public class ValidationFieldsDTO
    {
        public bool IsValid { get; set; }
        public List<ValidationResult> ErrorMessages { get; set; } = new List<ValidationResult>();
    }
}
