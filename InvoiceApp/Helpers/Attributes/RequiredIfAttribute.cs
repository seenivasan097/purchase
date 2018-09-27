using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace InvoiceApp.Helpers.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly RequiredAttribute _innerAttribute = new RequiredAttribute();

        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }
        /// <summary>
        /// its used to check whether the target property value is true or false
        /// </summary>
        /// <param name="dependentProperty"></param>
        /// <param name="targetValue"></param>
        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }
        /// <summary>
        /// its used to check validaation result is true or false and error details
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(DependentProperty);
          
            if (field != null)
            {
                // get the value of the dependent property
                var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

                // compare the value against the target value
                if ((dependentvalue == null && TargetValue == null) ||
                    (dependentvalue != null && dependentvalue.Equals(TargetValue)))
                {
                    // match => means we should try validating this field
                    if (!_innerAttribute.IsValid(value))
                        // validation failed - return an error
                        return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
        /// <summary>
        /// its used to check client level validation and get client exception details
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredif",
            };

            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);

            // find the value on the control we depend on;
            // if it's a bool, format it javascript style 
            // (the default is True or False!)
            var targetValue = (TargetValue ?? "").ToString();
            if (TargetValue.GetType() == typeof(bool))
                targetValue = targetValue.ToLower();

            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("targetvalue", targetValue);

            yield return rule;
        }
        /// <summary>
        /// its used to build properety id based on the context and data
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(DependentProperty);
            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            return depProp;
        }
    }
}