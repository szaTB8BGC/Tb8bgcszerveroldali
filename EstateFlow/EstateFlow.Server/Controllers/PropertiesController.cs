using EstateFlow.Server.Contracts.Meters;
using EstateFlow.Server.Contracts.Properties;
usingEstateFlow.Infrastructure.Data;
using  EstateFlow.Domain;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly EstateFlowDbContext _context;
        private readonly IValidator<CreatePropertyRequest> _createValidator;
        private readonly IValidator<UpdatePropertyRequest> _updateValidator;

        public PropertiesController(EstateFlowDbContext context, 
            IValidator<CreatePropertyRequest> createValidator, 
            IValidator<UpdatePropertyRequest> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyResponse>>> GetProperties()
        {
            var properties = await _context.Properties
                .ProjectToType<PropertyResponse>()
                .ToListAsync();
            return Ok(properties);
        }

        // GET: api/properties/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyResponse>> GetProperty(Guid id)
        {
            var property = await _context.Properties
                .Where(p => p.Id == id)
                .ProjectToType<PropertyResponse>()
                .FirstOrDefaultAsync();

            if (property is null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        // GET: api/properties/{id}/meters
        [HttpGet("{id}/meters")]
        public async Task<ActionResult<IEnumerable<MeterResponse>>> GetPropertyMeters(Guid id)
        {
            var propertyExists = await _context.Properties.AnyAsync(p => p.Id == id);

            if (!propertyExists)
            {
                return NotFound();
            }

            var meters = await _context.Meters
                .Where(m => m.PropertyId == id)
                .ProjectToType<MeterResponse>()
                .ToListAsync();

            return Ok(meters);
        }

        // POST: api/properties
        [HttpPost]
        public async Task<ActionResult<PropertyResponse>> CreateProperty(CreatePropertyRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }

            var property = new Property
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Name = request.Name,
                Address = request.Address,
                SquareMeters = request.SquareMeters,
                AdvertisementText = request.AdvertisementText,
                CityId = request.CityId
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            var response = await _context.Properties
                .Where(p => p.Id == property.Id)
                .ProjectToType<PropertyResponse>()
                .SingleAsync();

            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, response);
        }

        // PUT: api/properties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(Guid id, UpdatePropertyRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }

            var property = await _context.Properties.FindAsync(id);

            if (property is null)
            {
                return NotFound();
            }

            property.Name = request.Name;
            property.Address = request.Address;
            property.SquareMeters = request.SquareMeters;
            property.AdvertisementText = request.AdvertisementText;
            property.CityId = request.CityId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/properties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property is null)
            {
                return NotFound();
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
