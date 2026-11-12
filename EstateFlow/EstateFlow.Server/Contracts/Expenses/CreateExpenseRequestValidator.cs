usingEstateFlow.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Server.Contracts.Expenses
{
    public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
    {
        public CreateExpenseRequestValidator(EstateFlowDbContext context)
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
