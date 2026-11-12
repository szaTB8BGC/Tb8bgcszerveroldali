using EstateFlow.Server.Contracts.Meters;
usingEstateFlow.Infrastructure.Data;
using  EstateFlow.Domain;
using EstateFlow.Server.Services;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetersController : ControllerBase
    {
        private readonly EstateFlowDbContext _context;
        private readonly IMeterReadingService _meterReadingService;
        private readonly IValidator<SubmitMeterReadingRequest> _submitValidator;

        public MetersController(EstateFlowDbContext context, 
            IMeterReadingService meterReadingService, 
            IValidator<SubmitMeterReadingRequest> submitValidator)
        {
            _context = context;
            _meterReadingService = meterReadingService;
            _submitValidator = submitValidator;
        }

        // GET: api/meters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meter>>> GetMeters()
        {
            var meters = await _context.Meters
                .Include(m => m.UtilityType)
                .Include(m => m.Provider)
                .ToListAsync();

            return Ok(meters);
        }

        // GET: api/meters/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Meter>> GetMeter(Guid id)
        {
            var meter = await _context.Meters
                .Include(m => m.UtilityType)
                .Include(m => m.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meter is null)
            {
                return NotFound();
            }

            return Ok(meter);
        }

        // GET: api/meters/{id}/readings
        [HttpGet("{id}/readings")]
        public async Task<ActionResult<IEnumerable<MeterReading>>> GetMeterReadings(Guid id)
        {
            var meterExists = await _context.Meters.AnyAsync(m => m.Id == id);

            if (!meterExists)
            {
                return NotFound();
            }

            var readings = await _context.MeterReadings
                .Where(r => r.MeterId == id)
                .OrderBy(r => r.MeasuredAt)
                .ToListAsync();

            return Ok(readings);
        }

        // POST: api/meters/{id}/readings
        [HttpPost("{id}/readings")]
        public async Task<ActionResult<MeterReadingResponse>> SubmitReading(Guid id, SubmitMeterReadingRequest request)
        {
            var validationResult = await _submitValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }

            var result = await _meterReadingService.SubmitReadingAsync(id, request);

            if (result.MeterNotFound)
            {
                return NotFound();
            }

            if (result.Error is not null)
            {
                return Problem(result.Error, statusCode: StatusCodes.Status400BadRequest);
            }

            var response = result.Reading!.Adapt<MeterReadingResponse>();

            return CreatedAtAction(nameof(GetMeterReadings), new { id }, response);
        }
    }
}
