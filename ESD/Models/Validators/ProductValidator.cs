using FluentValidation;
using ESD.Models.Dtos;
using System.Text.RegularExpressions;

namespace ESD.Models.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.ProductCode).NotEmpty().WithMessage("product.productCode_required").WithMessage("product.Not_match_code");
            //RuleFor(s => s.ModelId).NotEmpty().WithMessage("product.model_required");
            //RuleFor(s => s.ProductType).NotEmpty().WithMessage("product.ProductType_required");
            RuleFor(s => s.ProductName).NotEmpty().WithMessage("product.ProductName_required");
            //RuleFor(s => s.ProjectName).NotEmpty().WithMessage("product.ProjectName_required");
            //RuleFor(s => s.SSVersion).NotEmpty().WithMessage("product.SSVersion_required");
            //RuleFor(s => s.ExpiryMonth).NotEmpty().WithMessage("product.ExpiryMonth_required");
            //RuleFor(s => s.Temperature).NotEmpty().WithMessage("product.Temperature_required");
            //RuleFor(s => s.Stamps).NotEmpty().WithMessage("product.Stamps_required");
            //RuleFor(s => s.PackingAmount).NotNull().GreaterThan(0)
            //      .ScalePrecision(3, 9)
            //    .WithMessage("product.InchFormatError");
        }
    }
}
