using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class Buyer2Validator : AbstractValidator<Buyer2Dto>
    {
        public Buyer2Validator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.BuyerCode).NotEmpty().WithMessage("buyer.BuyerCode_required");
            RuleFor(s => s.BuyerName).NotEmpty().WithMessage("buyer.BuyerName_required");
        }
    }
}
