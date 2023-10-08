using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class LocationValidator : AbstractValidator<LocationDto>
    {
        public LocationValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.LocationCode).NotEmpty().WithMessage("location.LocationCode_required");
            RuleFor(s => s.AreaCode).NotEmpty().WithMessage("location.AreaCode_required");
        }
    }
}
