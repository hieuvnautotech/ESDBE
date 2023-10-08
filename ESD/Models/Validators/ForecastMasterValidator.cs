using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class ForecastMasterValidator : AbstractValidator<ForecastPOMasterDto>
    {
        public ForecastMasterValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.FPoMasterCode).NotEmpty().WithMessage("forecast.FPO_Code_required");
        }
    }
}
