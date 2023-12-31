﻿using FluentValidation;
using ESD.Models.Dtos;

namespace ESD.Models.Validators
{
    public class QCDetailValidator : AbstractValidator<QCDetailDto>
    {
        public QCDetailValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.QCId).NotEmpty().WithMessage("qcDetail.QCCode_required");
            RuleFor(s => s.QCMasterId).NotEmpty().WithMessage("qcDetail.QCCode_required");
        }
    }
}
