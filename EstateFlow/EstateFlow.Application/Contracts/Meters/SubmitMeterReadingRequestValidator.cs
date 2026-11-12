using FluentValidation;

namespace EstateFlow.Application.Contracts.Meters
{
    public class SubmitMeterReadingRequestValidator : AbstractValidator<SubmitMeterReadingRequest>
    {
        public SubmitMeterReadingRequestValidator()
        {
            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MeasuredAt)
                .NotEmpty();
        }
    }
}
