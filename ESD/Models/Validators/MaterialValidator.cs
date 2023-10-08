using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class MaterialValidator : AbstractValidator<MaterialDto>
    {
        public MaterialValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.MaterialCode).NotEmpty().WithMessage("material.MaterialCode_required");
            RuleFor(s => s.MaterialUnit).NotEmpty().WithMessage("material.Unit_required");
        }
    }
}
