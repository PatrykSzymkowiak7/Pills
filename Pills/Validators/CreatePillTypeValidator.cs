using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pills.Models.ViewModels.PillTypes;

namespace Pills.Validators
{
    public class CreatePillTypeValidator : AbstractValidator<CreatePillTypeViewModel>
    {
        public CreatePillTypeValidator(AppDbContext appDbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name cannot be empty")
                .MinimumLength(3)
                    .WithMessage("Name must have more than 3 characters")
                .MaximumLength(50)
                    .WithMessage("Name must have less than 50 characters");

            RuleFor(x => x.MaxAllowed)
                .NotEmpty()
                    .WithMessage("Dose cannot be empty")
                .GreaterThan(0)
                    .WithMessage("Dose must be greater than 0")
                .LessThanOrEqualTo(10)
                    .WithMessage("Dose must be less than 10");

            RuleFor(x => x.Name)
                .MustAsync(async (name, ct) =>
                    !await appDbContext.PillsTypes.AnyAsync(p => p.Name == name))
                .WithMessage("Pill type with that name already exists");
        }
    }
}
