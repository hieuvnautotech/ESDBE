using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class MaterialSOMasterValidator : AbstractValidator<MaterialSOMasterDto>
    {
        public MaterialSOMasterValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(so => so.MsoCode)
                .NotEmpty().WithMessage("general.code_required")
                //.Length(36).WithMessage("lot.LotCode_length")
            ;

            RuleFor(so => so.Requester)
                .NotEmpty().WithMessage("material-so-master.requester_required")
            ;

            RuleFor(s => s.DueDate)
                .NotEmpty().WithMessage("material-so-master.DueDate_required")
                .Must(date => date != default(DateTime)).WithMessage("general.datetime_format_invalid")
            ;
        }
    }
}
