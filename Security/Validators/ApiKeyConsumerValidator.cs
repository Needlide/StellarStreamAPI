using FluentValidation;
using StellarStreamAPI.Security.POCOs;

namespace StellarStreamAPI.Security.Validators
{
    public class ApiKeyConsumerValidator : AbstractValidator<ApiKeyConsumer>
    {
        public ApiKeyConsumerValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email is not valid.").NotEmpty().WithMessage("Email is required.");
        }
    }
}
