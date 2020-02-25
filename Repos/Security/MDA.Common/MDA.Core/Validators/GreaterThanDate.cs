namespace MDA.Core.Validators
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class GreaterThanDate : ValidationAttribute, IClientValidatable
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
        /// Initializes a new instance of the GreaterThanDate class.
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <param name="allowEqualValues">Allow Equal Values</param>
        public GreaterThanDate(string fieldName, bool allowEqualValues)
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
                ValidationType = "greaterthandate"
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
            if (!(value is DateTime))
            {
                return ValidationResult.Success;
            }

            // if Field Value is Null return Success
            var fieldValue = fieldInformation.GetValue(validationContext.ObjectInstance, null);
            if (!(fieldValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare Values
            if ((DateTime)value >= (DateTime)fieldValue)
            {
                if (AllowEqualValues && (DateTime)value == (DateTime)fieldValue)
                {
                    return ValidationResult.Success;
                }
                else if ((DateTime)value > (DateTime)fieldValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}