using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class MaterialSODetailValidator : AbstractValidator<MaterialSODetailDto>
    {
        public MaterialSODetailValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.MsoId)
                .NotEmpty().WithMessage("material-so-detail.MsoId_required")
                .GreaterThan(0).WithMessage("material-so-detail.MsoId_required")
            ;
            RuleFor(s => s.MaterialId)
                .NotEmpty().WithMessage("material-so-detail.Material_required")
                .GreaterThan(0).WithMessage("material-so-detail.Material_required")
            ;
            RuleFor(s => s.SOrderQty)
                .NotEmpty().WithMessage("material-so-detail.SOrderQty_required")
                .GreaterThan(0).WithMessage("material-so-detail.SOrderQty_GreaterThan_0")
                .ScalePrecision(3, 10).WithMessage("material-so-detail.SQty_Format")
            ;
        }
    }
}
