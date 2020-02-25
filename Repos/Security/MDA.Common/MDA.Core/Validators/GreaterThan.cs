namespace MDA.Core.Validators
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class GreaterThan : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Gets or sets AllowEqualValues.
        /// </summary>
        private readonly bool AllowEqualValues;

        /// <summary>
        /// Gets or sets FieldName.
        /// </summary>
        private readonly string FieldName;

        /// <summary>
        /// Initializes a new instance of the GreaterThan class.
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <param name="allowEqualValues">Allow Equal Values</param>
        public GreaterThan(string fieldName, bool allowEqualValues)
        {
            FieldName = fieldName;
            AllowEqualValues = allowEqualValues;
        }

        /// <summary>
        /// Implement Interface Member of IClientValidatable
        /// </summary>
        /// <param name="modelMetadata">Model Metadata</param>
        /// <param name="controllerContext">Controller Context</param>
        /// <returns>Client Validation Rules</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(modelMetadata.GetDisplayName()),
                ValidationType = "greaterthan"
            };

            yield return modelClientValidationRule;
        }

        /// <summary>
        /// Override Base class version of IsValid
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="validationContext">Validation Context</param>
        /// <returns>Validation Result</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check Valid Property
            var fieldInformation = validationContext.ObjectType.GetProperty(FieldName);
            if (fieldInformation == null)
            {
                return new ValidationResult(string.Format("Unknown Property {0}", FieldName));
            }

            // if Value is Null return Success
            if (!(value is int))
            {
                return ValidationResult.Success;
            }

            // if Field Value is Null return Success
            var fieldValue = fieldInformation.GetValue(validationContext.ObjectInstance, null);
            if (!(fieldValue is int))
            {
                return ValidationResult.Success;
            }

            // Compare Values
            if ((int)value >= (int)fieldValue)
            {
                if (AllowEqualValues && (int)value == (int)fieldValue)
                {
                    return ValidationResult.Success;
                }
                else if ((int)value > (int)fieldValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}