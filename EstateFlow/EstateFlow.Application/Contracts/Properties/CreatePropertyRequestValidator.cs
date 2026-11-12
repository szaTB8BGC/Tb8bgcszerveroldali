using EstateFlow.Application;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Application.Contracts.Properties
{
    public class CreatePropertyRequestValidator : AbstractValidator<CreatePropertyRequest>
    {
        public CreatePropertyRequestValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(10)
                .MustAsync(async (code, cancellationToken) =>
                    !await context.Properties.AnyAsync(p => p.Code == code, cancellationToken))
                .WithMessage("A property with code '{PropertyValue}' already exists.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(200);

            RuleFor(x => x.SquareMeters)
                .GreaterThan(0);

            RuleFor(x => x.CityId)
                .MustAsync(async (cityId, cancellationToken) =>
                    await context.Cities.AnyAsync(c => c.Id == cityId, cancellationToken))
                .WithMessage("City with id {PropertyValue} does not exist.");
        }
    }
}