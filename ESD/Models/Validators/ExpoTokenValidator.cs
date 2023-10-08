using FluentValidation;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;

namespace ESD.Models.Validators
{
    public class ExpoTokenValidator : AbstractValidator<ExpoTokenDto>
    {
        public ExpoTokenValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.ExpoToken).NotEmpty().WithMessage("Token is required");
        }
    }
}
