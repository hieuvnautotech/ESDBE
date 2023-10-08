using ESD.Models.Dtos.MMS;
using FluentValidation;

namespace ESD.Models.Validators
{
    public class WOSemiLotMMSDetailValidator : AbstractValidator<WOSemiLotMMSDetailDto>
    {
        public WOSemiLotMMSDetailValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.WOSemiLotMMSId).NotEmpty().WithMessage("WO.WOSemiLotMMSId_required");
            RuleFor(s => s.MaterialLotCode).NotEmpty().WithMessage("WO.MaterialLotCode_required");
            RuleFor(s => s.WOProcessId).NotEmpty().WithMessage("WO.WOProcessId_required");
        }
    }
}
