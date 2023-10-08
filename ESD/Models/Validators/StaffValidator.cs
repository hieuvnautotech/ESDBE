using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class StaffValidator : AbstractValidator<StaffDto>
    {
        public StaffValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.StaffCode).NotEmpty().WithMessage("staff.StaffCode_required");
            RuleFor(s => s.StaffName).NotEmpty().WithMessage("staff.StaffName_required");
        }
    }
}
