using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class QCMasterValidator : AbstractValidator<QCMasterDto>
    {
        public QCMasterValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.QCMasterCode).NotEmpty().WithMessage("qcMaster.QCCode_required");
        }
    }
}
