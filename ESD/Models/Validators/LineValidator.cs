using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class LineValidator : AbstractValidator<LineDto>
    {
        public LineValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.LineName).NotEmpty().WithMessage("line.LineName_required");
        }
    }
}
