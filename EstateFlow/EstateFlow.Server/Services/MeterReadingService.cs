using EstateFlow.Server.Contracts.Meters;
usingEstateFlow.Infrastructure.Data;
using  EstateFlow.Domain;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Server.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly EstateFlowDbContext _context;

        public MeterReadingService(EstateFlowDbContext context)
        {
            _context = context;
        }

        public async Task<MeterReadingSubmissionResult> SubmitReadingAsync(Guid meterId, SubmitMeterReadingRequest request)
        {
            var meterExists = await _context.Meters.AnyAsync(m => m.Id == meterId);

            if (!meterExists)
            {
                return MeterReadingSubmissionResult.NotFound();
            }

            var measuredAt = request.MeasuredAt.Kind == DateTimeKind.Utc
                ? request.MeasuredAt
                : DateTime.SpecifyKind(request.MeasuredAt, DateTimeKind.Utc);

            // rule 1
            var latestReading = await _context.MeterReadings
                .Where(r => r.MeterId == meterId)
                .OrderByDescending(r => r.MeasuredAt)
                .FirstOrDefaultAsync();

            if (latestReading is not null && request.Value < latestReading.Value)
            {
                return MeterReadingSubmissionResult.Failure(FormattableString.Invariant(
                    $"The submitted value ({request.Value}) is lower than the latest reading ({latestReading.Value})."));
            }

            // rule 2
            if (latestReading is not null && measuredAt <= latestReading.MeasuredAt)
            {
                return MeterReadingSubmissionResult.Failure(
                    $"The measurement date ({measuredAt:yyyy-MM-dd}) must be later than the date of the latest reading ({latestReading.MeasuredAt:yyyy-MM-dd}).");
            }

            // rule 3
            var alreadyReportedThisMonth = await _context.MeterReadings.AnyAsync(r =>
                r.MeterId == meterId
                && r.MeasuredAt.Year == measuredAt.Year
                && r.MeasuredAt.Month == measuredAt.Month);

            if (alreadyReportedThisMonth)
            {
                return MeterReadingSubmissionResult.Failure(
                    $"A reading has already been reported for {measuredAt:yyyy-MM} on this meter.");
            }

            var reading = new MeterReading
            {
                Id = Guid.NewGuid(),
                MeterId = meterId,
                Value = request.Value,
                MeasuredAt = measuredAt,
                ReportedAt = DateTime.UtcNow
            };

            _context.MeterReadings.Add(reading);
            await _context.SaveChangesAsync();

            return MeterReadingSubmissionResult.Success(reading);
        }
    }
}
