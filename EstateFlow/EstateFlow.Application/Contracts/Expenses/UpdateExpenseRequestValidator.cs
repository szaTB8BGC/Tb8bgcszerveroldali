using EstateFlow.Application;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Application.Contracts.Expenses
{
    public class UpdateExpenseRequestValidator : AbstractValidator<UpdateExpenseRequest>
    {
        public UpdateExpenseRequestValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Type)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.PropertyId)
                .MustAsync(async (propertyId, cancellationToken) =>
                    await context.Properties.AnyAsync(p => p.Id == propertyId, cancellationToken))
                .WithMessage("Property with id {PropertyValue} does not exist.");
        }
    }
}