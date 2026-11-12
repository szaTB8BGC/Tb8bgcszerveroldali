using EstateFlow.Application.Contracts.Meters;

namespace EstateFlow.Application.Services
{
    public interface IMeterReadingService
    {
        Task<MeterReadingSubmissionResult> SubmitReadingAsync(Guid meterId, SubmitMeterReadingRequest request);
    }
}
