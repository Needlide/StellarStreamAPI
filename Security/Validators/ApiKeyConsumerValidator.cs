using FluentValidation;
using StellarStreamAPI.POCOs.Models.Security;

namespace StellarStreamAPI.Security.Validators
{
    public class ApiKeyConsumerValidator : AbstractValidator<ApiKeyConsumerRegistrationModel>
    {
        public ApiKeyConsumerValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email is not valid.").NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").Length(8, 256).WithMessage("Length must be between 8 and 256 characters.");
        }
    }
}
