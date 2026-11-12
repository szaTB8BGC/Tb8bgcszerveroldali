using EstateFlow.Application;
using Microsoft.AspNetCore.Mvc;

namespace EstateFlow.Server.Controllers
{
    [Route("api/meter-readings")]
    [ApiController]
    public class MeterReadingsController : ControllerBase
    {
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

        private readonly IApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MeterReadingsController(IApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // POST: api/meter-readings/{id}/photo
        [HttpPost("{id}/photo")]
        public async Task<IActionResult> UploadPhoto(Guid id, IFormFile? photo)
        {
            var reading = await _context.MeterReadings.FindAsync(id);

            if (reading is null)
            {
                return NotFound();
            }

            if (photo is null || photo.Length == 0)
            {
                return Problem("No file was uploaded.", statusCode: StatusCodes.Status400BadRequest);
            }

            if (photo.Length > MaxFileSizeBytes)
            {
                return Problem("The file exceeds the maximum allowed size of 5 MB.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                return Problem($"File type '{extension}' is not allowed. Allowed types: .jpg, .jpeg, .png.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var uploadsDirectory = Path.Combine(_environment.ContentRootPath, "uploads", "meter-readings");
            Directory.CreateDirectory(uploadsDirectory);

            var fileName = $"{reading.Id}{extension}";
            var filePath = Path.Combine(uploadsDirectory, fileName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await photo.CopyToAsync(stream);
            }

            reading.PhotoPath = $"uploads/meter-readings/{fileName}";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}