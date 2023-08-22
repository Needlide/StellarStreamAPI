using FluentValidation;
using StellarStreamAPI.POCOs;

namespace StellarStreamAPI.Security.Validators
{
    public class ApiKeyValidator : AbstractValidator<ApiKey>
    {
        public ApiKeyValidator()
        {
            RuleFor(x => x.KeyName).NotEmpty().WithMessage("Name of the API key is required.").Length(8, 48);
        }
    }
}
