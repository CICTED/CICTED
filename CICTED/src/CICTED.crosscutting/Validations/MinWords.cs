using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Crosscutting.Validations
{
    public class MinWords : ValidationAttribute
    {
        private int minWords;

        public MinWords(int minWords)
        {
            this.minWords = minWords;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.GetType() == Type.GetType("string"))
            {
                return new ValidationResult($"The attribute {validationContext.DisplayName} must be a string.");
            }

            if (value.ToString().Split(' ').Length > minWords)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}
