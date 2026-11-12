using  EstateFlow.Domain;

namespace EstateFlow.Server.Services
{
    public class MeterReadingSubmissionResult
    {
        public MeterReading? Reading { get; private init; }
        public string? Error { get; private init; }
        public bool MeterNotFound { get; private init; }

        public static MeterReadingSubmissionResult Success(MeterReading reading) => new() { Reading = reading };

        public static MeterReadingSubmissionResult NotFound() => new() { MeterNotFound = true };

        public static MeterReadingSubmissionResult Failure(string error) => new() { Error = error };
    }
}
