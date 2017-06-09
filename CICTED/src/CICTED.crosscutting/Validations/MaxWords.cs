using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Crosscutting.Validations
{
    public class MaxWords : ValidationAttribute
    {
        private int maxWords;

        public MaxWords(int maxWords)
        {
            this.maxWords = maxWords;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"The attribute {validationContext.DisplayName} can't be null.");
            
            if (value.GetType().FullName == "String")
            {
                return new ValidationResult($"The attribute {validationContext.DisplayName} must be a string.");
            }

            if (value.ToString().Split(' ').Length > maxWords)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}
