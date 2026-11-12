usingEstateFlow.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Server.Contracts.Properties
{
    public class UpdatePropertyRequestValidator : AbstractValidator<UpdatePropertyRequest>
    {
        public UpdatePropertyRequestValidator(EstateFlowDbContext context)
        {
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
