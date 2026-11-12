namespace EstateFlow.Application.Contracts.Expenses
{
    public class CreateExpenseRequest
    {
        public DateTime PaidAt { get; set; }
        public decimal Amount { get; set; }
        public required string Type { get; set; }
        public required string Name { get; set; }
        public string? Note { get; set; }
        public Guid PropertyId { get; set; }
    }
}
