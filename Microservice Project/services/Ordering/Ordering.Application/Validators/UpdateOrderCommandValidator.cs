using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(o => o.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("{Id} is required")
                .GreaterThan(0)
                .WithMessage("{Id} must be greater than 0");
            RuleFor(o => o.UserName)
                .NotEmpty()
                .NotNull()
                .WithMessage("{UserName} is required")
                .MaximumLength(70)
                .WithMessage("{UserName} must not exceed 70 characters");
            RuleFor(o => o.EmailAddress)
                .NotEmpty()
                .NotNull()
                .WithMessage("{EmailAddress} is required")
                .EmailAddress()
                .WithMessage("{EmailAddress} is not a valid email address");
            RuleFor(o => o.FirstName)
                .NotEmpty()
                .NotNull()
                .WithMessage("{FirstName} is required")
                .MaximumLength(50)
                .WithMessage("{FirstName} must not exceed 50 characters");
            RuleFor(o => o.LastName)
                .NotEmpty()
                .NotNull()
                .WithMessage("{LastName} is required")
                .MaximumLength(50)
                .WithMessage("{LastName} must not exceed 50 characters");
            RuleFor(o => o.TotalPrice)
                .NotNull()
                .GreaterThan(-1)
                .WithMessage("{TotalPrice} must be postive");
        }
    }
}
