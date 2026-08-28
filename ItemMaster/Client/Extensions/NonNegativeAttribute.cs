using System.ComponentModel.DataAnnotations;

namespace ItemMaster.Client.Extensions
{
	public class NonNegativeAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is decimal decimalValue && decimalValue < 0)
			{
				return new ValidationResult("Cannot set value minus");
			}
			return ValidationResult.Success;
		}
	}
}
