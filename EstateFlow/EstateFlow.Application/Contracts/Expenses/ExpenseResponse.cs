namespace EstateFlow.Application.Contracts.Expenses
{
    public class ExpenseResponse
    {
        public Guid Id { get; set; }
        public DateTime PaidAt { get; set; }
        public decimal Amount { get; set; }
        public required string Type { get; set; }
        public required string Name { get; set; }
        public string? Note { get; set; }
        public Guid PropertyId { get; set; }
        public required string PropertyName { get; set; }
    }
}
