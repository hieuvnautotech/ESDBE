using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class StandarQCValidator : AbstractValidator<StandardQCDto>
    {
        public StandarQCValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.QCCode).NotEmpty().WithMessage("standardQC.QCCode_required");
        }
    }
}
