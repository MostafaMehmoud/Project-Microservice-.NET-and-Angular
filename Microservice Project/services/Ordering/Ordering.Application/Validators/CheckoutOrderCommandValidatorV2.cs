using FluentValidation;
using Ordering.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderCommandValidatorV2 : AbstractValidator<CheckoutOrderCommandV2>
    {
        public CheckoutOrderCommandValidatorV2()
        {

            RuleFor(o=>o.UserName)
               .NotEmpty()
                .NotNull()
                .WithMessage("{UserName} is required")
                .MaximumLength(70)
                .WithMessage("{UserName} must not exceed 70 characters");

            RuleFor(o=>o.TotalPrice)
                .NotNull()
                .GreaterThan(-1)
                .WithMessage("{TotalPrice} must be postive");
        }
    }
}
