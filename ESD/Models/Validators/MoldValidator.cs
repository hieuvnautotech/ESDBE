using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class MoldValidator : AbstractValidator<MoldDto>
    {
        public MoldValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(s => s.MoldCode)
                .NotEmpty().WithMessage("mold.MoldSerial_required")
                ;

            RuleFor(s => s.MoldName)
                .NotEmpty().WithMessage("mold.MoldName_required");
        }
    }
}
