using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class BomValidator : AbstractValidator<BomDto>
    {
        public BomValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("bom.MaterialId_required");
        }
    }
}
