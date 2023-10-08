using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class HMICountingValidator : AbstractValidator<HMICountingDto>
    {
        public HMICountingValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.WoId)
                .NotEmpty()
                .GreaterThan(0)
            ;

            RuleFor(s => s.MoldId)
                .NotEmpty()
                .GreaterThan(0)
            ;

            RuleFor(s => s.HMIStatusName)
                .NotEmpty()
            ;
        }
    }
}
