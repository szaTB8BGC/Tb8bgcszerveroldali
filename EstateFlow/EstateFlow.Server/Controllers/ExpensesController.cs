using EstateFlow.Server.Contracts.Expenses;
using EstateFlow.Server.Data;
using  EstateFlow.Domain;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateFlow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly EstateFlowDbContext _context;
        private readonly IValidator<CreateExpenseRequest> _createValidator;
        private readonly IValidator<UpdateExpenseRequest> _updateValidator;

        public ExpensesController(EstateFlowDbContext context, 
            IValidator<CreateExpenseRequest> createValidator,
            IValidator<UpdateExpenseRequest> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseResponse>>> GetExpenses()
        {
            var expenses = await _context.Expenses
                .ProjectToType<ExpenseResponse>()
                .ToListAsync();

            return Ok(expenses);
        }

        // GET: api/expenses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseResponse>> GetExpense(Guid id)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id == id)
                .ProjectToType<ExpenseResponse>()
                .FirstOrDefaultAsync();

            if (expense is null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        // POST: api/expenses
        [HttpPost]
        public async Task<ActionResult<ExpenseResponse>> CreateExpense(CreateExpenseRequest request)
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

            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                PaidAt = request.PaidAt.Kind == DateTimeKind.Utc
                    ? request.PaidAt
                    : DateTime.SpecifyKind(request.PaidAt, DateTimeKind.Utc),
                Amount = request.Amount,
                Type = request.Type,
                Name = request.Name,
                Note = request.Note,
                PropertyId = request.PropertyId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var response = await _context.Expenses
                .Where(e => e.Id == expense.Id)
                .ProjectToType<ExpenseResponse>()
                .SingleAsync();

            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, response);
        }

        // PUT: api/expenses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(Guid id, UpdateExpenseRequest request)
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

            var expense = await _context.Expenses.FindAsync(id);

            if (expense is null)
            {
                return NotFound();
            }

            expense.PaidAt = request.PaidAt.Kind == DateTimeKind.Utc
                ? request.PaidAt
                : DateTime.SpecifyKind(request.PaidAt, DateTimeKind.Utc);
            expense.Amount = request.Amount;
            expense.Type = request.Type;
            expense.Name = request.Name;
            expense.Note = request.Note;
            expense.PropertyId = request.PropertyId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/expenses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense is null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
