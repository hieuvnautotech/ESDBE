using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class LotValidator : AbstractValidator<LotDto>
    {
        public LotValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            //RuleFor(s => s.LotCode)
            //    .NotEmpty().WithMessage("lot.LotCode_required")
            //    .Length(36).WithMessage("lot.LotCode_length")
            //;
            RuleFor(s => s.MaterialId)
                .NotEmpty().WithMessage("lot.Material_required")
                .GreaterThan(0).WithMessage("lot.Material_required")
            ;
            RuleFor(s => s.Qty)
                .NotEmpty().WithMessage("lot.Qty_required")
                .GreaterThan(0).WithMessage("lot.Qty_bigger_0")
                .ScalePrecision(3, 10).WithMessage("lot.Qty_Format")
            ;
            //RuleFor(s => s.QCDate)
            //    .NotEmpty().WithMessage("lot.QCDate_required")
            //    .Must(date => date != default(DateTime)).WithMessage("general.datetime_format_invalid")
            //;
        }
        
    }
}
