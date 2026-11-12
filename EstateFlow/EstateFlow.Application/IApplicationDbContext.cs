using EstateFlow.Domain;
using Microsoft.EntityFrameworkCore;
namespace EstateFlow.Application;

public interface IApplicationDbContext
{
    DbSet<City> Cities { get; }
    DbSet<Property> Properties { get; }
DbSet<PropertyImage> PropertyImages { get; }
    DbSet<Meter> Meters { get; }
    DbSet<UtilityType> UtilityTypes { get; }
    DbSet<Provider> Providers { get; }
    DbSet<MeterReading> MeterReadings { get; }
    DbSet<Tenant> Tenants { get; }
    DbSet<Rental> Rentals { get; }
    DbSet<Invoice> Invoices { get; }
    DbSet<Contract> Contracts { get; }
    DbSet<Expense> Expenses { get; }
    DbSet<Payment> Payments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}