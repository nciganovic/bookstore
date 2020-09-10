using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utilities
{
    public class EndGreaterThenStartDateAttribute : ValidationAttribute
    {
        private string otherPropertyName;

        public EndGreaterThenStartDateAttribute(string otherPropertyName, string errorMessage) : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;

            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(otherPropertyName);
            var startDateValue = field.GetValue(validationContext.ObjectInstance, null);
            DateTime startDate = (DateTime)startDateValue;

            if (startDateValue == null) {
                return new ValidationResult("Start date is empty");
            }
            if (field == null) {
                return new ValidationResult(string.Format("Unknown property: {0}.", otherPropertyName));
            }

            DateTime EndDate = (DateTime)value;

            if (startDate >= EndDate) {
                return new ValidationResult("Start date must be less then end date");
            }

            return validationResult;
        }
    }
}
