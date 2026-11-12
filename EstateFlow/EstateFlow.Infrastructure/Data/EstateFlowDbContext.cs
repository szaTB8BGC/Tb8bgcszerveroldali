using EstateFlow.Domain;
using Microsoft.EntityFrameworkCore;

using EstateFlow.Application;

namespace EstateFlow.Infrastructure.Data;

public class EstateFlowDbContext : DbContext, IApplicationDbContext
    {       

        public DbSet<City> Cities => Set<City>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
        public DbSet<Meter> Meters => Set<Meter>();
        public DbSet<UtilityType> UtilityTypes => Set<UtilityType>();
        public DbSet<Provider> Providers => Set<Provider>();
        public DbSet<MeterReading> MeterReadings => Set<MeterReading>();
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Rental> Rentals => Set<Rental>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Payment> Payments => Set<Payment>();

        public EstateFlowDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Meter>()
                .HasOne(m => m.UtilityType)
                .WithMany()
                .HasForeignKey(m => m.UtilityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Meter>()
                .HasOne(m => m.Provider)
                .WithMany()
                .HasForeignKey(m => m.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.UtilityType)
                .WithMany(t => t.Providers)
                .HasForeignKey(p => p.UtilityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Provider)
                .WithMany()
                .HasForeignKey(i => i.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MeterReading>()
                .HasOne(r => r.Meter)
                .WithMany(m => m.Readings)
                .HasForeignKey(r => r.MeterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rental>()
                .Property(r => r.MonthlyRent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MeterReading>()
                .Property(r => r.Value)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.RentAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.UtilitiesAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Budapest" },
                new City { Id = 2, Name = "Debrecen" },
                new City { Id = 3, Name = "Szeged" },
                new City { Id = 4, Name = "Kecskemét" }
            );

            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    Code = "BPSZK",
                    Name = "Szondi utcai lakás",
                    Address = "1068 Budapest, Szondi utca 45.",
                    SquareMeters = 54,
                    AdvertisementText = "Világos, felújított másfél szobás lakás a VI. kerület szívében, a Nyugati pályaudvartól 10 percre.",
                    CityId = 1
                },
                new Property
                {
                    Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    Code = "BPFKR",
                    Name = "Ferenc körúti lakás",
                    Address = "1092 Budapest, Ferenc körút 12.",
                    SquareMeters = 68,
                    AdvertisementText = null,
                    CityId = 1
                },
                new Property
                {
                    Id = new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"),
                    Code = "DEEGY",
                    Name = "Egyetem sugárúti lakás",
                    Address = "4026 Debrecen, Egyetem sugárút 8.",
                    SquareMeters = 47,
                    AdvertisementText = "Hangulatos, erkélyes lakás a Nagyerdő közelében, egyetemistáknak ideális.",
                    CityId = 2
                }
            );

            modelBuilder.Entity<PropertyImage>().HasData(
                new PropertyImage
                {
                    Id = new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                    Name = "Nappali",
                    FilePath = "uploads/properties/bpszk-nappali.jpg",
                    UploadedAt = new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new PropertyImage
                {
                    Id = new Guid("6ec0bd7f-11c0-43da-975e-2a8ad9ebae0b"),
                    Name = "Hálószoba",
                    FilePath = "uploads/properties/bpszk-haloszoba.jpg",
                    UploadedAt = new DateTime(2026, 1, 15, 10, 32, 0, DateTimeKind.Utc),
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new PropertyImage
                {
                    Id = new Guid("f9168c5e-ceb2-4faa-b6bf-329bf39fa1e4"),
                    Name = "Utcai homlokzat",
                    FilePath = "uploads/properties/bpfkr-homlokzat.jpg",
                    UploadedAt = new DateTime(2026, 2, 3, 14, 0, 0, DateTimeKind.Utc),
                    PropertyId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")
                },
                new PropertyImage
                {
                    Id = new Guid("d2719b1e-1c3b-4e5f-9a6d-8b7c6d5e4f3a"),
                    Name = "Konyha",
                    FilePath = "uploads/properties/deegy-konyha.jpg",
                    UploadedAt = new DateTime(2026, 2, 10, 9, 15, 0, DateTimeKind.Utc),
                    PropertyId = new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d")
                }
            );

            modelBuilder.Entity<UtilityType>().HasData(
                new UtilityType { Id = 1, Name = "Electricity" },
                new UtilityType { Id = 2, Name = "Gas" },
                new UtilityType { Id = 3, Name = "Water" }
            );

            modelBuilder.Entity<Provider>().HasData(
                new Provider { Id = 1, Name = "MVM Next", Note = null, UtilityTypeId = 1 },
                new Provider { Id = 2, Name = "FŐGÁZ", Note = "a gázórákat évente egyszer olvassák le", UtilityTypeId = 2 },
                new Provider { Id = 3, Name = "Fővárosi Vízművek", Note = null, UtilityTypeId = 3 },
                new Provider { Id = 4, Name = "Debreceni Vízmű", Note = null, UtilityTypeId = 3 }
            );

            modelBuilder.Entity<Meter>().HasData(
                new Meter
                {
                    Id = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"),
                    Name = "Villanyóra",
                    Location = "előszobai szekrényben",
                    SerialNumber = "EL-2019-448723",
                    ReadingPeriod = "minden hónap elején",
                    Note = null,
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    UtilityTypeId = 1,
                    ProviderId = 1
                },
                new Meter
                {
                    Id = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3302"),
                    Name = "Gázóra",
                    Location = "konyhában a bejárati ajtó mellett",
                    SerialNumber = "GA-2017-902211",
                    ReadingPeriod = null,
                    Note = "2027-ben hitelesítés esedékes",
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    UtilityTypeId = 2,
                    ProviderId = 2
                },
                new Meter
                {
                    Id = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3303"),
                    Name = "Vízóra",
                    Location = "fürdőszobában a kád alatt",
                    SerialNumber = "VZ-2021-115847",
                    ReadingPeriod = "minden páros hónapban van leolvasás",
                    Note = null,
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    UtilityTypeId = 3,
                    ProviderId = 3
                },
                new Meter
                {
                    Id = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3304"),
                    Name = "Villanyóra",
                    Location = "folyosón a bejárati ajtó mellett",
                    SerialNumber = "EL-2020-771139",
                    ReadingPeriod = "minden hónap elején",
                    Note = null,
                    PropertyId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    UtilityTypeId = 1,
                    ProviderId = 1
                },
                new Meter
                {
                    Id = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3305"),
                    Name = "Vízóra",
                    Location = "konyhában a mosogató alatt",
                    SerialNumber = "VZ-2018-660254",
                    ReadingPeriod = null,
                    Note = null,
                    PropertyId = new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"),
                    UtilityTypeId = 3,
                    ProviderId = 4
                }
            );

            modelBuilder.Entity<MeterReading>().HasData(
                new MeterReading
                {
                    Id = new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c001"),
                    MeasuredAt = new DateTime(2026, 3, 1, 8, 0, 0, DateTimeKind.Utc),
                    Value = 12450m,
                    ReportedAt = new DateTime(2026, 3, 1, 8, 5, 0, DateTimeKind.Utc),
                    ApprovedAt = new DateTime(2026, 3, 2, 9, 0, 0, DateTimeKind.Utc),
                    PhotoPath = "uploads/readings/bpszk-villany-2026-03.jpg",
                    MeterId = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301")
                },
                new MeterReading
                {
                    Id = new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c002"),
                    MeasuredAt = new DateTime(2026, 4, 1, 7, 40, 0, DateTimeKind.Utc),
                    Value = 12588m,
                    ReportedAt = new DateTime(2026, 4, 1, 7, 45, 0, DateTimeKind.Utc),
                    ApprovedAt = new DateTime(2026, 4, 2, 10, 15, 0, DateTimeKind.Utc),
                    PhotoPath = "uploads/readings/bpszk-villany-2026-04.jpg",
                    MeterId = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301")
                },
                new MeterReading
                {
                    Id = new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c003"),
                    MeasuredAt = new DateTime(2026, 5, 1, 9, 30, 0, DateTimeKind.Utc),
                    Value = 12731m,
                    ReportedAt = new DateTime(2026, 5, 3, 18, 20, 0, DateTimeKind.Utc),
                    ApprovedAt = new DateTime(2026, 5, 4, 8, 30, 0, DateTimeKind.Utc),
                    PhotoPath = null,
                    MeterId = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301")
                },
                new MeterReading
                {
                    Id = new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c004"),
                    MeasuredAt = new DateTime(2026, 6, 1, 8, 10, 0, DateTimeKind.Utc),
                    Value = 12876m,
                    ReportedAt = new DateTime(2026, 6, 1, 9, 10, 0, DateTimeKind.Utc),
                    ApprovedAt = null,
                    PhotoPath = "uploads/readings/bpszk-villany-2026-06.jpg",
                    MeterId = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301")
                },
                new MeterReading
                {
                    Id = new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c005"),
                    MeasuredAt = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                    Value = 341.605m,
                    ReportedAt = new DateTime(2026, 4, 1, 10, 5, 0, DateTimeKind.Utc),
                    ApprovedAt = new DateTime(2026, 4, 3, 11, 0, 0, DateTimeKind.Utc),
                    PhotoPath = "uploads/readings/bpszk-viz-2026-04.jpg",
                    MeterId = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3303")
                },
                new MeterReading
                {
                    Id = new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c006"),
                    MeasuredAt = new DateTime(2026, 6, 1, 10, 0, 0, DateTimeKind.Utc),
                    Value = 348.112m,
                    ReportedAt = new DateTime(2026, 6, 2, 16, 45, 0, DateTimeKind.Utc),
                    ApprovedAt = null,
                    PhotoPath = "uploads/readings/bpszk-viz-2026-06.jpg",
                    MeterId = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3303")
                }
            );

            modelBuilder.Entity<Tenant>().HasData(
                new Tenant
                {
                    Id = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                    FirstName = "Anna",
                    LastName = "Kovács",
                    Email = "kovacs.anna@example.com",
                    PhoneNumber = "+36 20 123 4567"
                },
                new Tenant
                {
                    Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                    FirstName = "Péter",
                    LastName = "Szabó",
                    Email = "szabo.peter@example.com",
                    PhoneNumber = null
                }
            );

            modelBuilder.Entity<Rental>().HasData(
                new Rental
                {
                    Id = new Guid("0b28a2f0-762c-4b7f-8b3e-9a5d1c4e6f01"),
                    StartDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = null,
                    MonthlyRent = 240000m,
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    TenantId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538")
                },
                new Rental
                {
                    Id = new Guid("0b28a2f0-762c-4b7f-8b3e-9a5d1c4e6f02"),
                    StartDate = new DateTime(2025, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 31, 0, 0, 0, DateTimeKind.Utc),
                    MonthlyRent = 150000m,
                    PropertyId = new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"),
                    TenantId = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479")
                }
            );

            modelBuilder.Entity<Expense>().HasData(
                new Expense
                {
                    Id = new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2001"),
                    PaidAt = new DateTime(2026, 3, 12, 14, 20, 0, DateTimeKind.Utc),
                    Amount = 28500m,
                    Type = "Repair",
                    Name = "Vízvezeték-javítás a fürdőszobában",
                    Note = null,
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new Expense
                {
                    Id = new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2002"),
                    PaidAt = new DateTime(2026, 4, 5, 9, 0, 0, DateTimeKind.Utc),
                    Amount = 18000m,
                    Type = "Maintenance",
                    Name = "Gázkazán éves karbantartása",
                    Note = "kötelező éves felülvizsgálat",
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new Expense
                {
                    Id = new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2003"),
                    PaidAt = new DateTime(2026, 4, 20, 11, 30, 0, DateTimeKind.Utc),
                    Amount = 45200m,
                    Type = "Tax",
                    Name = "Építményadó, I. félév",
                    Note = null,
                    PropertyId = new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d")
                },
                new Expense
                {
                    Id = new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2004"),
                    PaidAt = new DateTime(2026, 5, 8, 16, 45, 0, DateTimeKind.Utc),
                    Amount = 12900m,
                    Type = "Maintenance",
                    Name = "Zárcsere a bejárati ajtón",
                    Note = null,
                    PropertyId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")
                }
            );

            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    Id = new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3001"),
                    PaidAt = new DateTime(2026, 3, 3, 10, 15, 0, DateTimeKind.Utc),
                    RentAmount = 240000m,
                    UtilitiesAmount = 36500m,
                    PeriodYear = 2026,
                    PeriodMonth = 3,
                    Note = null,
                    TenantId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new Payment
                {
                    Id = new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3002"),
                    PaidAt = new DateTime(2026, 4, 2, 8, 40, 0, DateTimeKind.Utc),
                    RentAmount = 240000m,
                    UtilitiesAmount = 41200m,
                    PeriodYear = 2026,
                    PeriodMonth = 4,
                    Note = null,
                    TenantId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new Payment
                {
                    Id = new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3003"),
                    PaidAt = new DateTime(2026, 5, 4, 19, 5, 0, DateTimeKind.Utc),
                    RentAmount = 240000m,
                    UtilitiesAmount = 33800m,
                    PeriodYear = 2026,
                    PeriodMonth = 5,
                    Note = "két nap késéssel érkezett",
                    TenantId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new Payment
                {
                    Id = new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3004"),
                    PaidAt = new DateTime(2026, 6, 2, 12, 0, 0, DateTimeKind.Utc),
                    RentAmount = 240000m,
                    UtilitiesAmount = 30150m,
                    PeriodYear = 2026,
                    PeriodMonth = 6,
                    Note = null,
                    TenantId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                    PropertyId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
                },
                new Payment
                {
                    Id = new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3005"),
                    PaidAt = new DateTime(2026, 5, 10, 7, 55, 0, DateTimeKind.Utc),
                    RentAmount = 150000m,
                    UtilitiesAmount = 22400m,
                    PeriodYear = 2026,
                    PeriodMonth = 5,
                    Note = null,
                    TenantId = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                    PropertyId = new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d")
                },
                new Payment
                {
                    Id = new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3006"),
                    PaidAt = new DateTime(2026, 6, 9, 17, 30, 0, DateTimeKind.Utc),
                    RentAmount = 150000m,
                    UtilitiesAmount = 21750m,
                    PeriodYear = 2026,
                    PeriodMonth = 6,
                    Note = null,
                    TenantId = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                    PropertyId = new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d")
                }
            );
        }
    }

